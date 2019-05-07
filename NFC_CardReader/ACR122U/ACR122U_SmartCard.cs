using System;
using System.Collections.Generic;
using System.Text;
using NFC_CardReader.CppToCSharpConversionHelpers;

namespace NFC_CardReader.ACR122U
{
    public class ACR122U_SmartCard : WinSmartCard
    {
        /// <summary>
        /// The last error specific to the ACR122
        /// </summary>
        public ACR122U_ResposeErrorCodes LastACRResultCode { get; protected set; }


        public ACR122U_SmartCard(WinSmartCard MakeFrom) : this(MakeFrom.Parent, MakeFrom.Card)
        {}

        ACR122U_SmartCard(WinSmartCardContext Parent, int Card) : base(Parent, Card)
        {}

        #region DeviceUtilities

        ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data)
        {
            byte[] OpReturn = new byte[4] { Data[Data.Length - 1], Data[Data.Length - 2], 0, 0 };
            Array.Resize(ref Data, Data.Length - 2);
            return (ACR122U_ResposeErrorCodes)BitConverter.ToInt32(OpReturn, 0);
        }

        ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data, out byte TrailingResposeData)
        {
            byte[] OpReturn = new byte[4] { Data[Data.Length - 1], Data[Data.Length - 2], 0, 0 };
            Array.Resize(ref Data, Data.Length - 2);
            if (OpReturn[1] == 0x90)
            {
                TrailingResposeData = OpReturn[0];
                OpReturn[0] = 0;
            }
            else
                TrailingResposeData = 0;
            return (ACR122U_ResposeErrorCodes)BitConverter.ToInt32(OpReturn, 0);
        }

        public static string GetACRErrMsg(ACR122U_ResposeErrorCodes ReturnCode)
        {
            switch (ReturnCode)
            {
                case ACR122U_ResposeErrorCodes.Success:
                    return "The action was Successful.";
                case ACR122U_ResposeErrorCodes.Error:
                    return "The action was Canceled due to Error. Please Call status to get error";
                case ACR122U_ResposeErrorCodes.FuctionNotSupported:
                    return "The action could not be executed as the fuction is not supported by your current hardware or firmware";
                case ACR122U_ResposeErrorCodes.WinSCardError:
                    return "An error occurred before the ACR122u at the Winscard.dll";
                default:
                    return "?";
            }
        }
        #endregion

        #region NotSupportedWinscardFuctions
        /// <summary>
        /// Do not call these functions from here as they are not supported by this platform
        /// </summary>
        /// <param name="Attribute"></param>
        /// <param name="AttrOut"></param>
        /// <param name="IsBytes"></param>
        /// <returns></returns>
        public override ErrorCodes GetAttrib(SmartCardATR Attribute, out byte[] AttrOut)
        {
            throw new ACR122U_SmartCardException(ACR122U_ResposeErrorCodes.FuctionNotSupported, ErrorCodes.SCARD_S_OperationNotSupported);
        }

        /// <summary>
        /// Do not call these functions from here as they are not supported by this platform
        /// </summary>
        /// <param name="Attribute"></param>
        /// <param name="AttrOut"></param>
        /// <param name="IsBytes"></param>
        /// <returns></returns>
        public override ErrorCodes GetAttrib(SmartCardATR Attribute, out string AttrOut, bool IsBytes = false)
        {
            throw new ACR122U_SmartCardException(ACR122U_ResposeErrorCodes.FuctionNotSupported, ErrorCodes.SCARD_S_OperationNotSupported);
        }
        #endregion

        #region DeviceSpecificCommands

        /*Turn On/Off anntenna (Couldnt Figure out what this was actually doing out actually)
        *Send 
        FF 00 00 00 04 D4 [000<?:1=on,0=off>]
        *Returns
        D5 33 90 00 for success(90 00)
        */
        /// <summary>
        /// Turns RFID anntenna On
        /// </summary>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes TurnAnntennaOn()
        {
            byte[] DataOut;
            const ulong _TurnAnntennaOn = 0xFF00000004D40100;
            byte[] CommandAsBytes = BitConverter.GetBytes(_TurnAnntennaOn);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 7);

            TransmitData(CommandAsBytes, out DataOut);

            return RetrieveDataCodes(ref DataOut);
        }

        /*Turn On/Off anntenna (Couldnt Figure out what this was actually doing out actually)
        *Send 
        FF 00 00 00 04 D4 [000<?:1=on,0=off>]
        *Returns
        D5 33 90 00 for success(90 00)
        */
        /// <summary>
        /// Turns RFID anntenna off
        /// </summary>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes TurnAnntennaOff()
        {
            byte[] DataOut;
            const ulong _TurnAnntennaOff = 0xFF00000004D40000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_TurnAnntennaOff);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 7);

            TransmitData(CommandAsBytes, out DataOut);

            return RetrieveDataCodes(ref DataOut);
        }

        /*Get PICC Operating Parameter state
        *Send 
        FF 00 50 00 00
        *Returns
        90 [State(see bellow)] for success(90 00)
        ->State as
        [AutoPICCPolling:1=Enable,0=Disable]
        [AutoATSGeneration:1=Enable,0=Disable]
        [PollingInterval:1=250ms,0=500ms]
        [Felica424K:1=Detect,0=Ignore]
        [Felica212K:1=Detect,0=Ignore]
        [Topaz:1=Detect,0=Ignore]
        [ISO14443TypeB:1=Detect,0=Ignore]
        [ISO14443TypeA:1=Detect,0=Ignore]
        */
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
        public ACR122U_ResposeErrorCodes GetPICCOperatingParameterState(out byte DataOut)
        {
            byte[] Data;
            const ulong _GetPICCOperatingParameterState = 0xFF00500000000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetPICCOperatingParameterState);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);

            TransmitData(CommandAsBytes, out Data);

            return RetrieveDataCodes(ref Data, out DataOut);
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
        public ACR122U_ResposeErrorCodes GetPICCOperatingParameterState(out ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            byte SetInData;
            ACR122U_ResposeErrorCodes Return = GetPICCOperatingParameterState(out SetInData);
            SetInDataOut = (ACR122U_PICCOperatingParametersControl)SetInData;
            return Return;
        }


        /*Set PICC Operating Parameter state
        *Send 
        FF 00 51 [State(see bellow)] 00
        *Returns
        90 [State(see bellow)] for success(90 00)
        ->State as
        [AutoPICCPolling(0x80):1=Enable,0=Disable]
        [AutoATSGeneration(0x40):1=Enable,0=Disable]
        [PollingInterval(0x20):1=250ms,0=500ms]
        [Felica424K(0x10):1=Detect,0=Ignore]
        [Felica212K(0x08):1=Detect,0=Ignore]
        [Topaz(0x04):1=Detect,0=Ignore]
        [ISO14443TypeB(0x02):1=Detect,0=Ignore]
        [ISO14443TypeA(0x01):1=Detect,0=Ignore]
        */
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
        public ACR122U_ResposeErrorCodes SetPICCOperatingParameterState(ref byte SetInDataOut)
        {
            byte[] Data;
            const ulong _SetPICCOperatingParameterState = 0xFF00510000000000;
            //                                 Set All On 0xFF0051FF00000000;//Sets all off (build up with use of above enum)
            //                                       FF<<32  =|   FF00000000;
            ulong SetPICCOperatingParameterState = _SetPICCOperatingParameterState | ((ulong)SetInDataOut << 32);
            byte[] CommandAsBytes = BitConverter.GetBytes(SetPICCOperatingParameterState);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);

            TransmitData(CommandAsBytes, out Data);

            return RetrieveDataCodes(ref Data, out SetInDataOut);
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
        public ACR122U_ResposeErrorCodes SetPICCOperatingParameterState(ref ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            byte SetInData = (byte)SetInDataOut;
            ACR122U_ResposeErrorCodes Return = SetPICCOperatingParameterState(ref SetInData);
            SetInDataOut = (ACR122U_PICCOperatingParametersControl)SetInData;
            return Return;
        }


        /*Set PICC Operating Parameter state
        *Send 
        FF 00 40 [LED&Buzzer(see bellow)] 04 [T1Duration(see bellow)] [T2Duration(see bellow)] [TimesToRepeatCycle(see bellow)] [BuzzerSettings(see bellow)]
        *Returns
        90 [Changes and Im not sure what it means (+1 for NoCard?)] for success(90 00)
        ->LED&Buzzer as                          ->T1Duration as
        [GreenBlinkingMask(0x80):1=Blink,0=Dont]       [T1Duration:00-FF(as value x 100ms)]
        [RedBlinkingMask(0x40):1=Blink,0=Dont]         ->T2Duration as
        [InitialGreenBlinkingState(0x20):1=On,0=Off]   [T2Duration:00-FF(as value x 100ms)]
        [InitialRedBlinkingState(0x10):1=On,0=Off]     [TimesToRepeatCycle:00-FF]
        [GreenLEDStateMask(0x08):1=Update,0=Dont]      ->BuzzerSettings as
        [RedLEDStateMask(0x04):1=Update,0=Dont]        <FirstByteUnused:0s on postions FC>
        [GreenFinalState(0x02):1=On,0=Off]             [BuzzerOnT1Cycle(0x02):1=On,0=Off]
        [RedFinalState(0x01):1=On,0=Off]               [BuzzerOnT12Cycle(0x01):1=On,0=Off]
        */
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
        public ACR122U_ResposeErrorCodes SetLEDandBuzzerControl(ACR122U_LEDControl LEDControl, byte T1Duration, byte T2Durration, byte TimesToRepeat, ACR122U_BuzzerControl BuzzerControl, out byte DataOut)
        {
            byte[] Data;
            const ulong _SetLEDandBuzzerControl = 0xFF00400004000000;//current state sets all off.     
            //    Set All On with Max time |0xFF0040FF04FFFF03;//Sets all off (build up with use of above enums and time input)
            ulong SetLEDandBuzzerControl = _SetLEDandBuzzerControl;
            SetLEDandBuzzerControl |= ((ulong)LEDControl) << 32;
            SetLEDandBuzzerControl |= ((ulong)T1Duration) << 16;
            SetLEDandBuzzerControl |= ((ulong)T1Duration) << 8;
            SetLEDandBuzzerControl |= (ulong)BuzzerControl;

            byte[] CommandAsBytes = BitConverter.GetBytes(SetLEDandBuzzerControl);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 8);

            TransmitData(CommandAsBytes, out Data);

            return RetrieveDataCodes(ref Data, out DataOut);
        }

        /*Get Status
        *Send 
        FF 00 00 00 02 D4 04
        *Returns
        D5 05 [ErrorCode] [Field] [NumberOfTargets] [LogicalNumber] [BitRateInReception] [BitRateInTransmission] [ModulationType] 80 90 00
        Or for no card D5 05 00 00 00 80 90 00 00 00 00 00
        ->Error Code as Unknown               
        [Errorcodes not decemitated fully]
        ->Field as                           
        [Field:0x00=RF field not Present]   
        ->Number of Target as
        [NumberOfTargets:00-FF(only seen 1 with card)]
        ->Bits Rate In Reception as
        [BitsRateInTransmiton:(0x)00=106kbps,01=212kbps,02=424kbps]  
        ->Bits Rate In Transmiton as
        [BitsRateInTransmiton:(0x)00=106kbps,01=212kbps,02=424kbps]  
        ->Modulation Type as
        [ModulationType:(0x)00=ISO1443orMifare,01=ActiveMode,02=InnovisionJewelTag]
        */
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
        public ACR122U_ResposeErrorCodes GetStatus(out bool Card, out ACR122U_StatusErrorCodes ErrorCode, out bool FieldPresent, out byte NumberOfTargets, out byte LogicalNumber, out ACR122U_StatusBitRateInReception BitRateInReception, out ACR122U_StatusBitsRateInTransmiton BitRateInTransmition, out ACR122U_StatusModulationType ModulationType)
        {
            byte[] Data;
            const ulong _GetStatus = 0xFF00000002D40400;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetStatus);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 7);

            TransmitData(CommandAsBytes, out Data);
            //D5 05[ErrorCode][Field][NumberOfTargets][LogicalNumber][BitRateInReception][BitRateInTransmission][ModulationType] 80 90 00
            //D5 05[00][00][00][00][00][00][00] 80 90 00 Card
            //D5 05 00 00 00 80 90 00 00 00 00 00 no Card
            ErrorCode = (ACR122U_StatusErrorCodes)Data[2];
            FieldPresent = Data[3] != 0;
            NumberOfTargets = Data[4];
            if (Data[9]== 0x80 && Data[10] == 0x90 && Data[11] == 0x00)
            {
                Card = true;
                LogicalNumber = Data[5];
                BitRateInReception = (ACR122U_StatusBitRateInReception)Data[6];
                BitRateInTransmition = (ACR122U_StatusBitsRateInTransmiton)Data[7];
                ModulationType = (ACR122U_StatusModulationType)Data[8];

                return ACR122U_ResposeErrorCodes.Success;
            }
            else if (Data[5] == 0x80 && Data[6] == 0x90 && Data[7] == 0x00)
            {
                Card = false;
                LogicalNumber = 0;
                BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
                BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
                ModulationType = ACR122U_StatusModulationType.NoCardDetected;

                return ACR122U_ResposeErrorCodes.Success;
            }

            Card = false;
            LogicalNumber = 0;
            BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
            BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
            ModulationType = ACR122U_StatusModulationType.NoCardDetected;
            return RetrieveDataCodes(ref Data); //RetrieveDataCodes(ref DataOut);
        }




        const ulong _GetStatus = 0xFF00000002D40400;

        #endregion
        #region CardCommandsAllFor1KForNow

        /*Get UID 
         *Send
         FF CA 00 00 00
         *Returns
         [Data] [Data] [Data] [Data] [SWCodeP1] [SWCodeP2]
         */
        /// <summary>
        /// Gets the UID as bytes
        /// </summary>
        /// <param name="receivedUID">the UID</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes GetcardUIDBytes(out byte[] receivedUID)//only for mifare 1k cards
        {
            const ulong _GetcardUIDBytesCommand = 0xFFCA000000000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);

            TransmitData(CommandAsBytes, out receivedUID);

            return RetrieveDataCodes(ref receivedUID);
        }

        /// <summary>
        /// Gets the UID as as a string
        /// </summary>
        /// <returns></returns>
        public string GetcardUID()//only for mifare 1k cards
        {

            string cardUID = "";
            byte[] receivedUID;

            LastACRResultCode = GetcardUIDBytes(out receivedUID);

            if (LastACRResultCode != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(LastACRResultCode, LastResultCode);

            cardUID = BitConverter.ToString(receivedUID);  //CppToCSharpHelpers.StringsFromNullTerminatedByteBuffer(receivedUID);

            return cardUID;
        }

        /*Get ATS 
         *Send
         FF CA 01 00 00
         *Returns
         [Data] [Data] [Data] [Data] [SWCodeP1] [SWCodeP2]
         */
         /// <summary>
         /// Gets The ATS Current particuar hardware or firmware or card doesnt support
         /// </summary>
         /// <param name="receivedUID"></param>
         /// <returns></returns>
        public ACR122U_ResposeErrorCodes GetcardATSBytes(out byte[] receivedUID)//only for mifare 1k cards
        {
            const ulong _GetcardUIDBytesCommand = 0xFFCA010000000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);

            TransmitData(CommandAsBytes, out receivedUID);

            return RetrieveDataCodes(ref receivedUID);
        }

        /// <summary>
        /// Gets The ATS Current particuar hardware or firmware or card doesnt support
        /// </summary>
        /// <param name="receivedUID"></param>
        /// <returns></returns>
        public string GetcardATS()//only for mifare 1k cards
        {

            string cardUID = "";
            byte[] receivedUID;

            LastACRResultCode = GetcardATSBytes(out receivedUID);

            if (LastACRResultCode != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(LastACRResultCode, LastResultCode);

            cardUID = BitConverter.ToString(receivedUID);  //CppToCSharpHelpers.StringsFromNullTerminatedByteBuffer(receivedUID);

            return cardUID;
        }

        /*Load Key
        *Send 
        FF 82 00 00 06 FF FF FF FF FF FF
        or
        FF 82 [KeyStructure:00(all others are reserved for fu)] [KeyNumber:00-01] 06 [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**]
        *Returns
        90 00
        */
        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as 1 or 2 for posible memory locations</param>
        /// <param name="KeyValue">The Key Value to use. Length must be six</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes LoadAthenticationKeys(ACR122U_KeyMemories Key, byte[] KeyValue)//only for mifare 1k cards
        {
            byte[] Return;
            if (KeyValue.Length != 6)
                throw new Exception("Your Key has too many or too few bites.\nKeys must have 6 bytes.");
            const ulong _GetcardUIDBytesCommand = 0xFF82000006000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 11);
            CommandAsBytes[3] = (byte)Key;
            CommandAsBytes[5] = KeyValue[0];
            CommandAsBytes[6] = KeyValue[1];
            CommandAsBytes[7] = KeyValue[2];
            CommandAsBytes[8] = KeyValue[3];
            CommandAsBytes[9] = KeyValue[4];
            CommandAsBytes[10] = KeyValue[5];

            TransmitData(CommandAsBytes, out Return);

            return RetrieveDataCodes(ref Return);
        }

        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <param name="KeyValue">The Value Key to use. Length must be six</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes LoadAthenticationKeys(ACR122U_Keys Key, byte[] KeyValue)
        {
            return LoadAthenticationKeys((ACR122U_KeyMemories)Key, KeyValue);
        }

        /*Load Key
        *Send 
        FF 86 00 00 05 01 00 00 60 01
        or
        FF 86 00 00 05 01 [Block:00-FF] 6{000<KeyType:0=A,1=B>} [KeyNumber:00-01]
        *Returns
        90 00
        Note in a 4 block sector the last block is the key block
        */
        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="KeyToUse">A enumeration as 1 or 2 for posible memory locations</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_KeyMemories KeyToUse)
        {
            byte[] Return;//                        FF860000050100006001
            const ulong _GetcardUIDBytesCommand = 0xFF86000005010000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 10);
            CommandAsBytes[6] = BlockToAthenticate;
            CommandAsBytes[8] = (byte)(0x60 | (byte)Key);
            CommandAsBytes[9] = (byte)KeyToUse; 

            TransmitData(CommandAsBytes, out Return);

            return RetrieveDataCodes(ref Return);
        }

        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_Keys KeyToUse)
        {
            return Athentication( BlockToAthenticate, Key, (ACR122U_KeyMemories)KeyToUse);
        }

        /*Read Bock
        *Send 
        FF B0 00 51 10
        or
        FF B0 00 [Block:00-FF] [NumberToRead:00-16]
        *Returns
        90 00
        */
        /// <summary>
        /// Reads a block
        /// </summary>
        /// <param name="DataOut">The data returned</param>
        /// <param name="BlockToRead">The block to read</param>
        /// <param name="NumberToRead">The number to read</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes ReadBlock(out byte[] DataOut, byte BlockToRead, byte NumberToRead = 16)
        {
            if (NumberToRead > 16 || NumberToRead < 0)// only one they let you with? :-/
                throw new Exception("Your Data In has too or too few bites.\nRead commands must have 0-16 bytes.");
            const ulong _GetcardUIDBytesCommand = 0xFFB0000000000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);
            CommandAsBytes[3] = BlockToRead;
            CommandAsBytes[4] = NumberToRead;

            TransmitData(CommandAsBytes, out DataOut);

            return RetrieveDataCodes(ref DataOut);
        }

        /*WriteBlock
        *Send 
        FF B0 00 51 10
        or
        FF 86 00 00 [Block:00-FF] 6{000<KeyType:0=A,1=B>} [KeyNumber:00-01]
        *Returns
        90 00
        */
        /// <summary>
        /// Writes to a block. Note Must be athenticated with Key B first
        /// </summary>
        /// <param name="DataIn">Data to write </param>
        /// <param name="BlockToWrite">The Block to write the data to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes WriteBlock(byte[] DataIn, byte BlockToWrite)
        {
            byte[] Return;
            if (DataIn.Length != 16)
                throw new Exception("Your Data In has too or too few bites.\nWrite commands must have 16 bytes.");
            const ulong _GetcardUIDBytesCommand = 0xFFB0000000000000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5 + DataIn.Length);
            CommandAsBytes[3] = BlockToWrite;
            CommandAsBytes[4] = (byte)DataIn.Length;
            int i = 0;
            foreach(byte b in DataIn)
            {
                CommandAsBytes[5 + i] = DataIn[i];
                i += 1;
            }

            TransmitData(CommandAsBytes, out Return);
            return RetrieveDataCodes(ref Return);
        }

        /*ReadValue
        *Send 
        FF B1 00 34 04
        or
        FF B1 00 [Block] 04
        *Returns
        90 00
        */
        /// <summary>
        /// Writes a 16 bit interger to the block
        /// </summary>
        /// <param name="Value">The number output</param>
        /// <param name="BlockToRead">The Block to Read from</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes ReadValueFromBlock(out short Value, byte BlockToRead)
        {
            ACR122U_ResposeErrorCodes Return;
            byte[] DataBack;
            //                                      FFD700000500FFFFFFFF
            const ulong _GetcardUIDBytesCommand = 0xFFD700000500000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 5);
            CommandAsBytes[3] = BlockToRead;

            TransmitData(CommandAsBytes, out DataBack);
            Return = RetrieveDataCodes(ref DataBack);
            Array.Reverse(DataBack);
            Value = BitConverter.ToInt16(DataBack, 0);
            return Return;
        }

        /*WriteValue
        *Send 
        FF D7 00 00 05 00 FF FF FF FF
        or
        FF D7 00 [Block] 05 00 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
        *Returns
        90 00
        */
        /// <summary>
        /// Writes a 16 bit interger to the block
        /// </summary>
        /// <param name="Value">the number to write</param>
        /// <param name="BlockToWrite">The Block to write to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes WriteValueToBlock(short Value, byte BlockToWrite)
        {
            byte[] Return;
            byte[] ValuesBytes;
            //                                      FFD700000500FFFFFFFF
            const ulong _GetcardUIDBytesCommand = 0xFFD700000500000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 10);
            ValuesBytes = BitConverter.GetBytes(Value);
            CommandAsBytes[3] = BlockToWrite;
            CommandAsBytes[6] = ValuesBytes[3];
            CommandAsBytes[7] = ValuesBytes[2];
            CommandAsBytes[8] = ValuesBytes[1];
            CommandAsBytes[9] = ValuesBytes[0];

            TransmitData(CommandAsBytes, out Return);
            return RetrieveDataCodes(ref Return);
        }

        /*IncrementValue
        *Send 
        FF D7 00 00 05 01 FF FF FF FF
        or
        FF D7 00 [Block] 05 01 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
        *Returns
        90 00
        */
        /// <summary>
        /// Increment number
        /// </summary>
        /// <param name="Value">The amount to add</param>
        /// <param name="BlockToIncrement">The block to add to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes IncrementValue(short Value, byte BlockToIncrement)
        {
            byte[] Return;
            byte[] ValuesBytes;
            //                                      FFD700000501FFFFFFFF
            const ulong _GetcardUIDBytesCommand = 0xFFD700000501000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 10);
            ValuesBytes = BitConverter.GetBytes(Value);
            CommandAsBytes[3] = BlockToIncrement;
            CommandAsBytes[6] = ValuesBytes[3];
            CommandAsBytes[7] = ValuesBytes[2];
            CommandAsBytes[8] = ValuesBytes[1];
            CommandAsBytes[9] = ValuesBytes[0];

            TransmitData(CommandAsBytes, out Return);
            return RetrieveDataCodes(ref Return);
        }

        /*IncrementValue
       *Send 
       FF D7 00 00 05 02 FF FF FF FF
       or
       FF D7 00 [Block] 05 02 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
       *Returns
       90 00
       */
        /// <summary>
        /// decrement number
        /// </summary>
        /// <param name="Value">The amount to subtract</param>
        /// <param name="BlockToDecrement">The block to subtract to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes DecrementValue(short Value, byte BlockToDecrement)
        {
            byte[] Return;
            byte[] ValuesBytes;
            //                                      FFD700000502FFFFFFFF
            const ulong _GetcardUIDBytesCommand = 0xFFD700000502000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 10);
            ValuesBytes = BitConverter.GetBytes(Value);
            CommandAsBytes[3] = BlockToDecrement;
            CommandAsBytes[6] = ValuesBytes[3];
            CommandAsBytes[7] = ValuesBytes[2];
            CommandAsBytes[8] = ValuesBytes[1];
            CommandAsBytes[9] = ValuesBytes[0];

            TransmitData(CommandAsBytes, out Return);
            return RetrieveDataCodes(ref Return);
        }

        /*Copy
        *Send 
        FF D7 00 00 02 03 00
        or
        FF D7 00 [SourceBlock] 02 03 [TargetBlock]
        *Returns
        90 00
        */
        /// <summary>
        /// Copies one block to another
        /// </summary>
        /// <param name="SourceBlock">Source Block copy from</param>
        /// <param name="TargetBlock">Target Block copy to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Copy(byte SourceBlock, byte TargetBlock)
        {
            byte[] Return;
            const ulong _GetcardUIDBytesCommand = 0xFFD7000002030000;
            byte[] CommandAsBytes = BitConverter.GetBytes(_GetcardUIDBytesCommand);
            Array.Reverse(CommandAsBytes);
            Array.Resize(ref CommandAsBytes, 7);
            CommandAsBytes[3] = SourceBlock;
            CommandAsBytes[6] = TargetBlock;

            TransmitData(CommandAsBytes, out Return);
            return RetrieveDataCodes(ref Return);
        }

        #endregion
        //not 100% sure this section is need but Ill throw it here as taken from a second dialog after identification
        #region ISO1443OrMifareCardCommands

        #endregion


    }
}
