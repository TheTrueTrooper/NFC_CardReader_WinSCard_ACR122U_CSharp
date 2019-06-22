using NFC_CardReader.ACR122U;
using NFC_CardReader.WinSCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122UManager
{
    /// <summary>
    /// A class to wrap the winscard and ACR122 API in a easy to use single unit as overall API and manage the card grabbing
    /// </summary>
    public class ACR122UManager : IDisposable
    {
        /// <summary>
        /// a basic event for if a card readers state has changed (card placed, timed out extra)
        /// </summary>
        public ACRCardStateChangeEventHandler CardStateChanged;

        /// <summary>
        /// the event handler for if a card is accepted by the LocalCardCheckOverRide (or GlobalCardCheck if not set) when CheckCard is set to true
        /// </summary>
        public ACRCardAcceptedCardScanEventHandler AcceptedCardScaned;

        /// <summary>
        /// the event handler for if a card is rejected by the LocalCardCheckOverRide (or GlobalCardCheck if not set) when CheckCard is set to true
        /// </summary>
        public ACRCardRejectedCardScanEventHandler RejectedCardScaned;

        /// <summary>
        /// the event handler for if a card is removed 
        /// </summary>
        public ACRCardRemovedEventHandler CardRemoved;

        /// <summary>
        /// the event handler for if a card is detected 
        /// </summary>
        public ACRCardDetectedEventHandler CardDetected;

        /// <summary>
        /// Our overall card reader context
        /// </summary>
        public WinSmartCardContext Context { private set; get; }

        /// <summary>
        /// the Reader we are using and its name
        /// </summary>
        public string ReaderName { private set; get; }

        /// <summary>
        /// the listing and polling thread
        /// </summary>
        Thread ListenerThread;

        /// <summary>
        /// The value to allow the other listening thread to die gracefully
        /// </summary>
        bool LetListenerThreadEnd = false;

        /// <summary>
        /// the Time out on blocking call polling
        /// </summary>
        int TimeOut;

        /// <summary>
        /// should we use the reject and accept functions with the global/local check functions set as a gate
        /// </summary>
        public bool CheckCard = false;

        /// <summary>
        /// The card that is in the machine
        /// </summary>
        public ACR122U_SmartCard Card { private set; get; }

        /// <summary>
        /// A global check we can set if we have a global scope for checking
        /// </summary>
        public static Func<ACRCardStateChangeEventArg, bool> GlobalCardCheck = (e) => throw new NotImplementedException("GlobalCardCheck or LocalCardCheckOverRide has not been set on ACR122UManager. Please ensure these have been set prior to turning on the CardCheck bool.");

        /// <summary>
        /// the private local storage for fuction pointer for the check
        /// </summary>
        Func<ACRCardStateChangeEventArg, bool> _LocalCardCheckOverRide = null;

        /// <summary>
        /// A accessor for the local card check that will defualt to the global if not set
        /// </summary>
        public Func<ACRCardStateChangeEventArg, bool> LocalCardCheckOverRide
        {
            get
            {
                if (_LocalCardCheckOverRide == null)
                    return GlobalCardCheck;
                return _LocalCardCheckOverRide;
            }

            set => _LocalCardCheckOverRide = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetACR122UReaders()
        {
            List<string> Names = WinSmartCardContext.ListReadersAsStringsStatic();
            Names = Names.Where(x => x.Contains("ACS ACR122") && !x.Contains("ACS ACR122U PICC Interface")).ToList();
            return Names;
        }

        /// <summary>
        /// Creates a new manager and starts a polling thread to listen for changes
        /// </summary>
        /// <param name="ReaderName">the reader to use</param>
        /// <param name="TimeOut">The poll rate in ms</param>
        public ACR122UManager(string ReaderName, int TimeOut = 1000)
        {
            this.ReaderName = ReaderName;
            this.TimeOut = TimeOut;
            CardStateChanged += CardStateChangedFunction;
            Context = new WinSmartCardContext(OperationScopes.SCARD_SCOPE_SYSTEM, ReaderName);
            ListenerThread = new Thread(new ParameterizedThreadStart(ListenerThreadFunction));
            ListenerThread.Start(this);
        }

        #region ConnectingCalls
        /// <summary>
        /// The use to connect with card and use.
        /// </summary>
        public void ConnectToCard()
        {
            //Context.Dispose();
            //Context = null;
            //Context = new WinSmartCardContext(OperationScopes.SCARD_SCOPE_SYSTEM, ReaderName);
            Card = new ACR122U_SmartCard(Context.CardConnect(SmartCardShareTypes.SCARD_SHARE_SHARED));
        }

        /// <summary>
        /// The use to disconnect with card.
        /// </summary>
        public void DisconnectToCard()
        {
            Card?.Dispose();
            Card = null;
        }
        #endregion

        #region CardReaderSudoADPUReaderComands
        /// <summary>
        /// Turns RFID anntenna On
        /// </summary>
        /// <returns></returns>
        public void TurnAnntennaOn()
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.TurnAnntennaOnStatic(Context);
            else
                Error = Card.TurnAnntennaOn();
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Turns RFID anntenna off
        /// </summary>
        /// <returns></returns>
        public void TurnAnntennaOff()
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.TurnAnntennaOffStatic(Context);
            else
                Error = Card.TurnAnntennaOff();
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Gets the Opperating params of system
        /// </summary>
        /// <param name="DataOut"> A byte as
        /// [AutoPICCPolling:1=Enable,0=Disable]
        /// [AutoATSGeneration:1=Enable,0=Disable]
        /// [PollingInterval:1=250ms,0=500ms]
        /// [Felica424K:1=Detect,0=Ignore]
        /// [Felica212K:1=Detect,0=Ignore]
        /// [Topaz:1=Detect,0=Ignore]
        /// [ISO14443TypeB:1=Detect,0=Ignore]
        /// [ISO14443TypeA:1=Detect,0=Ignore]
        /// </param>
        /// <returns></returns>
        public void GetPICCOperatingParameterState(out byte DataOut)
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.GetPICCOperatingParameterStateStatic(Context, out DataOut);
            else
                Error = Card.GetPICCOperatingParameterState(out DataOut);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Gets the Opperating params of system
        /// </summary>
        /// <param name="DataOut"> A enumerated byte as
        /// [AutoPICCPolling:1=Enable,0=Disable]
        /// [AutoATSGeneration:1=Enable,0=Disable]
        /// [PollingInterval:1=250ms,0=500ms]
        /// [Felica424K:1=Detect,0=Ignore]
        /// [Felica212K:1=Detect,0=Ignore]
        /// [Topaz:1=Detect,0=Ignore]
        /// [ISO14443TypeB:1=Detect,0=Ignore]
        /// [ISO14443TypeA:1=Detect,0=Ignore]
        /// </param>
        /// <returns></returns>
        public void GetPICCOperatingParameterState(out ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.GetPICCOperatingParameterStateStatic(Context, out SetInDataOut);
            else
                Error = Card.GetPICCOperatingParameterState(out SetInDataOut);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Sets and returns the Opperating params of system
        /// </summary>
        /// <param name="DataOut"> A byte as
        /// [AutoPICCPolling:1=Enable,0=Disable]
        /// [AutoATSGeneration:1=Enable,0=Disable]
        /// [PollingInterval:1=250ms,0=500ms]
        /// [Felica424K:1=Detect,0=Ignore]
        /// [Felica212K:1=Detect,0=Ignore]
        /// [Topaz:1=Detect,0=Ignore]
        /// [ISO14443TypeB:1=Detect,0=Ignore]
        /// [ISO14443TypeA:1=Detect,0=Ignore]
        /// </param>
        /// <returns></returns>
        public void SetPICCOperatingParameterState(ref byte SetInDataOut)
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.SetPICCOperatingParameterStateStatic(Context, ref SetInDataOut);
            else
                Error = Card.SetPICCOperatingParameterState(ref SetInDataOut);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Sets and returns the Opperating params of system
        /// </summary>
        /// <param name="DataOut"> A enumerated byte as
        /// [AutoPICCPolling:1=Enable,0=Disable]
        /// [AutoATSGeneration:1=Enable,0=Disable]
        /// [PollingInterval:1=250ms,0=500ms]
        /// [Felica424K:1=Detect,0=Ignore]
        /// [Felica212K:1=Detect,0=Ignore]
        /// [Topaz:1=Detect,0=Ignore]
        /// [ISO14443TypeB:1=Detect,0=Ignore]
        /// [ISO14443TypeA:1=Detect,0=Ignore]
        /// </param>
        /// <returns></returns>
        public void SetPICCOperatingParameterState(ref ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.SetPICCOperatingParameterStateStatic(Context, ref SetInDataOut);
            else
                Error = Card.SetPICCOperatingParameterState(ref SetInDataOut);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Sets the buzzer and LEDs to work in a T1 and T2 cycle like in a alarm
        /// </summary>
        /// <param name="LEDControl"> A enumberated byte as
        /// [GreenBlinkingMask(0x80):1=Blink,0=Dont]
        /// [RedBlinkingMask(0x40):1=Blink,0=Dont]
        /// [InitialGreenBlinkingState(0x20):1=On,0=Off]
        /// [InitialRedBlinkingState(0x10):1=On,0=Off]
        /// [GreenLEDStateMask(0x08):1=Update,0=Dont]
        /// [RedLEDStateMask(0x04):1=Update,0=Dont]    
        /// [GreenFinalState(0x02):1=On,0=Off]
        /// [BuzzerOnT1Cycle(0x02):1=On,0=Off]
        /// [RedFinalState(0x01):1=On,0=Off]   </param>
        /// <param name="T1Duration">T1Duration byte(as value x 100ms)</param>
        /// <param name="T2Durration">T2Duration byte(as value x 100ms)</param>
        /// <param name="TimesToRepeat">The Number of times that It sould repeat both cycles</param>
        /// <param name="BuzzerControl"></param> A enumberated byte as
        /// [BuzzerOnT1Cycle(0x02):1=On,0=Off]
        /// [RedFinalState(0x01):1=On,0=Off]
        /// [BuzzerOnT12Cycle(0x01):1=On,0=Off]
        /// <param name="DataOut">Some strange data that to this day I'm not sure of is incons only consi is the card comes on and of is +1</param>
        /// <returns></returns>
        public void SetLEDandBuzzerControl(ACR122U_LEDControl LEDControl, byte T1Duration, byte T2Durration, byte TimesToRepeat, ACR122U_BuzzerControl BuzzerControl, out byte DataOut)
        {
            ACR122U_ResposeErrorCodes Error;
            if (Card == null)
                Error = ACR122U_SmartCard.SetLEDandBuzzerControlStatic(Context, LEDControl, T1Duration, T2Durration, TimesToRepeat, BuzzerControl, out DataOut);
            else
                Error = Card.SetLEDandBuzzerControl(LEDControl, T1Duration, T2Durration, TimesToRepeat, BuzzerControl, out DataOut);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Gets the Status from the ACR122 using its internal method
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="ErrorCode"></param>
        /// <param name="FieldPresent"></param>
        /// <param name="NumberOfTargets"></param>
        /// <param name="LogicalNumber"></param>
        /// <param name="BitRateInReception"></param>
        /// <param name="BitRateInTransmition"></param>
        /// <param name="ModulationType"></param>
        /// <returns></returns>
        public void GetStatus(out bool Card, out ACR122U_StatusErrorCodes ErrorCode, out bool FieldPresent, out byte NumberOfTargets, out byte LogicalNumber, out ACR122U_StatusBitRateInReception BitRateInReception, out ACR122U_StatusBitsRateInTransmiton BitRateInTransmition, out ACR122U_StatusModulationType ModulationType)
        {
            ACR122U_ResposeErrorCodes Error;
            if (this.Card == null)
                Error = ACR122U_SmartCard.GetStatusStatic(Context, out Card, out ErrorCode, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitRateInTransmition, out ModulationType);
            else
                Error = this.Card.GetStatus(out Card, out ErrorCode, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitRateInTransmition, out ModulationType);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Gets the Status from the ACR122 using its internal method
        /// </summary>
        /// <param name="ACR122U_Status">A container with all of the status</param>
        /// <returns></returns>
        public void GetStatus(out ACR122U_Status ACR122U_Status)
        {
            ACR122U_ResposeErrorCodes Error;
            if (this.Card == null)
                Error = ACR122U_SmartCard.GetStatusStatic(Context, out ACR122U_Status);
            else
                Error = this.Card.GetStatus(out ACR122U_Status);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }
        #endregion

        #region MafireClassics
        #region Utilities
        /// <summary>
        /// Gets the UID as bytes
        /// </summary>
        /// <param name="receivedUID">the UID</param>
        /// <returns></returns>
        public void GetcardUIDBytes(out byte[] receivedUID)//only for mifare 1k cards
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.GetcardUIDBytes(out receivedUID);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Gets the UID as as a string
        /// </summary>
        /// <returns></returns>
        public string GetcardUID()//only for mifare 1k cards
        {
            return Card.GetcardUID();
        }

        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as 1 or 2 for posible memory locations</param>
        /// <param name="KeyValue">The Key Value to use. Length must be six</param>
        /// <returns></returns>
        public void LoadAthenticationKeys(ACR122U_KeyMemories Key, byte[] KeyValue)//only for mifare 1k cards
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.LoadAthenticationKeys(Key, KeyValue);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <param name="KeyValue">The Value Key to use. Length must be six</param>
        /// <returns></returns>
        public void LoadAthenticationKeys(ACR122U_Keys Key, byte[] KeyValue)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.LoadAthenticationKeys(Key, KeyValue);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="KeyToUse">A enumeration as 1 or 2 for posible memory locations</param>
        /// <returns></returns>
        public void Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_KeyMemories KeyToUse)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.Athentication(BlockToAthenticate, Key, KeyToUse);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <returns></returns>
        public void Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_Keys KeyToUse)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.Athentication(BlockToAthenticate, Key, KeyToUse);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }
        #endregion

        #region BlockRead&Writes
        /// <summary>
        /// Reads a block
        /// </summary>
        /// <param name="DataOut">The data returned</param>
        /// <param name="BlockToRead">The block to read</param>
        /// <param name="NumberToRead">The number to read</param>
        /// <returns></returns>
        public void ReadBlock(out byte[] DataOut, byte BlockToRead, byte NumberToRead = 16)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.ReadBlock(out DataOut, BlockToRead, NumberToRead);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Writes to a block. Note Must be athenticated with Key B first
        /// </summary>
        /// <param name="DataIn">Data to write </param>
        /// <param name="BlockToWrite">The Block to write the data to</param>
        /// <returns></returns>
        public void WriteBlock(byte[] DataIn, byte BlockToWrite)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.WriteBlock(DataIn, BlockToWrite);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }
        #endregion

        #region ValueRead&Writes
        //note all Value commands require a value to have been written to the location
        /// <summary>
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">The number output</param>
        /// <param name="BlockToRead">The Block to Read from</param>
        /// <returns></returns>
        // dont believe the compiler it lies my int is bigger.....
        public void ReadValueFromBlock(out Int32 Value, byte BlockToRead)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.ReadValueFromBlock(out Value, BlockToRead);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }
        /// <summary>
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">the number to write</param>
        /// <param name="BlockToWrite">The Block to write to</param>
        /// <returns></returns>
        public void WriteValueToBlock(Int32 Value, byte BlockToWrite)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.WriteValueToBlock(Value, BlockToWrite);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Increment number
        /// </summary>
        /// <param name="Value">The amount to add</param>
        /// <param name="BlockToIncrement">The block to add to</param>
        /// <returns></returns>
        public void IncrementValue(Int32 Value, byte BlockToIncrement)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.IncrementValue(Value, BlockToIncrement);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// decrement number
        /// </summary>
        /// <param name="Value">The amount to subtract</param>
        /// <param name="BlockToDecrement">The block to subtract to</param>
        /// <returns></returns>
        public void DecrementValue(Int32 Value, byte BlockToDecrement)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.DecrementValue(Value, BlockToDecrement);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }

        /// <summary>
        /// Copies one block to another
        /// </summary>
        /// <param name="SourceBlock">Source Block copy from</param>
        /// <param name="TargetBlock">Target Block copy to</param>
        /// <returns></returns>
        public void Copy(byte SourceBlock, byte TargetBlock)
        {
            if (Card == null)
                throw new Exception("Card is not connected.");
            ACR122U_ResposeErrorCodes Error;
            Error = Card.Copy(SourceBlock, TargetBlock);
            if (Error != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(Error, ErrorCodes.SCARD_S_SUCCESS);
        }
        #endregion
        #endregion

        /// <summary>
        /// A private thread to use for polling
        /// </summary>
        /// <param name="This"></param>
        static void ListenerThreadFunction(object This)
        {
            //set everting up and pass our selves in
            ACR122UManager Manager = (ACR122UManager)This;

            ReadersCurrentState[] States;

            lock (Manager)
                States = new ReadersCurrentState[] { new ReadersCurrentState() { ReaderName = Manager.ReaderName } };

            bool LetEnd = false;

            while (!LetEnd)
            {
                //cpp blocking call untill the readers state has changed.
                while (States[0].CurrentState == States[0].EventState)
                {
                    lock (Manager.Context)
                        Manager.Context.GetStatusChange(Manager.TimeOut, ref States);
                }

                //if the state has changed notify everyone
                lock (Manager.CardStateChanged)
                    Manager.CardStateChanged.Invoke(Manager, new ACRCardStateChangeEventArg(Manager, States[0]));

                //prepare for next check
                States[0].CurrentState = States[0].EventState;

                //check to see if we should let the thread die
                lock (Manager)
                   LetEnd = Manager.LetListenerThreadEnd;
            }
        }

        /// <summary>
        /// an private event for calling the other events on a state change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CardStateChangedFunction(object sender, ACRCardStateChangeEventArg e)
        {
            //on state change
            //check if there is a card
            if (e.ATR == null || e.ATR.Count() == 0)
                CardRemoved?.Invoke(sender, new ACRCardRemovedEventArg(this, e));
            else
                CardDetected?.Invoke(sender, new ACRCardDetectedEventArg(this, e));
            //check if we want the card
            if (CheckCard && LocalCardCheckOverRide.Invoke(e))
                AcceptedCardScaned?.Invoke(sender, new ACRCardAcceptedCardScanEventArg(this, e));
            else if (CheckCard && e.ATR != null && e.ATR.Count() > 0)
                RejectedCardScaned?.Invoke(sender, new ACRCardRejectedCardScanEventArg(this, e));
        }

        /// <summary>
        /// A way for you to print out the error messages
        /// </summary>
        /// <param name="ReturnCode">The code to check</param>
        /// <returns>a satement to its meening</returns>
        public static string GetACRErrMsg(ACR122U_ResposeErrorCodes ReturnCode)
        {
            return ACR122U_SmartCard.GetACRErrMsg(ReturnCode);
        }

        /// <summary>
        /// the disposal interface
        /// </summary>
        public void Dispose()
        {
            LetListenerThreadEnd = true;
            Context?.Dispose();
            Card?.Dispose();
        }
    }
}
