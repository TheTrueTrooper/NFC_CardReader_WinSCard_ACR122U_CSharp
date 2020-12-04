using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NFC_CardReader.ACR122U.CardTypes;
using NFC_CardReader.CppToCSharpConversionHelpers;
using NFC_CardReader.WinSCard;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// a API that wraps the Wincard API
    /// </summary>
    public abstract class ACR122U_SmartCard : WinSmartCard
    {
        /// <summary>
        /// The type of card that we are using
        /// </summary>
        public ACR122U_SupportedRFCardTypes CardType { get; protected set; }

        /// <summary>
        /// The last error specific to the ACR122
        /// </summary>
        public ACR122U_ResposeErrorCodes LastACRResultCode { get; protected set; }


        protected internal ACR122U_SmartCard(WinSmartCard MakeFrom) : this(MakeFrom.Parent, MakeFrom.Card)
        {}

        internal protected ACR122U_SmartCard(WinSmartCardContext Parent, IntPtr Card) : base(Parent, Card)
        {}

        #region DeviceUtilities

        internal protected static ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data)
        {
            byte[] OpReturn = new byte[4] { Data[Data.Length - 1], Data[Data.Length - 2], 0, 0 };
            Array.Resize(ref Data, Data.Length - 2);
            return (ACR122U_ResposeErrorCodes)BitConverter.ToInt32(OpReturn, 0);
        }
        //0xd5 0x41 0x27
        internal protected static ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data, out byte TrailingResposeData)
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

        #region DeviceSpecificCommandsPseudoAPDU

        #region CardLessVers

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
        public static ACR122U_ResposeErrorCodes GetPICCOperatingParameterStateStatic(WinSmartCardContext Context, out byte DataOut)
        {
            bool HasCard;
            byte[] Data;

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0x00, 0x50, 0x00, 0x00 };

            Context.Control(CommandAsBytes, out Data, out HasCard);

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
        public static ACR122U_ResposeErrorCodes GetPICCOperatingParameterStateStatic(WinSmartCardContext Context, out ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            byte SetInData;
            ACR122U_ResposeErrorCodes Return = GetPICCOperatingParameterStateStatic(Context, out SetInData);
            SetInDataOut = (ACR122U_PICCOperatingParametersControl)SetInData;
            return Return;
        }


        /// <summary>
        /// Sets and returns the Opperating params of system with out need of card
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
        public static ACR122U_ResposeErrorCodes SetPICCOperatingParameterStateStatic(WinSmartCardContext Context, ref byte SetInDataOut)
        {
            bool HasCard;
            byte[] Data;

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0x00, 0x51, SetInDataOut, 0x00 };

            Context.Control(CommandAsBytes, out Data, out HasCard);

            return RetrieveDataCodes(ref Data, out SetInDataOut);
        }

        /// <summary>
        /// Sets and returns the Opperating params of system with out need of card
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
        public static ACR122U_ResposeErrorCodes SetPICCOperatingParameterStateStatic(WinSmartCardContext Context, ref ACR122U_PICCOperatingParametersControl SetInDataOut)
        {
            byte SetInData = (byte)SetInDataOut;
            ACR122U_ResposeErrorCodes Return = SetPICCOperatingParameterStateStatic(Context, ref SetInData);
            SetInDataOut = (ACR122U_PICCOperatingParametersControl)SetInData;
            return Return;
        }

        /// <summary>
        /// Sets the buzzer and LEDs to work in a T1 and T2 cycle like in a alarm with out need for card
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
        public static ACR122U_ResposeErrorCodes SetLEDandBuzzerControlStatic(WinSmartCardContext Context, ACR122U_LEDControl LEDControl, byte T1Duration, byte T2Durration, byte TimesToRepeat, ACR122U_BuzzerControl BuzzerControl, out byte DataOut)
        {
            if (!Enum.IsDefined(typeof(ACR122U_BuzzerControl), BuzzerControl))
                throw new Exception("Your BuzzerControl selection was not valid.");

            bool HasCard;
            byte[] Data;

            byte[] CommandAsBytes = new byte[8] { 0xFF, 0x00, 0x40, (byte)LEDControl, 0x04, T1Duration, T1Duration, (byte)BuzzerControl };

            Context.Control(CommandAsBytes, out Data, out HasCard);

            return RetrieveDataCodes(ref Data, out DataOut);
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
        public static ACR122U_ResposeErrorCodes GetStatusStatic(WinSmartCardContext Context, out bool Card, out ACR122U_StatusErrorCodes ErrorCode, out bool FieldPresent, out byte NumberOfTargets, out byte LogicalNumber, out ACR122U_StatusBitRateInReception BitRateInReception, out ACR122U_StatusBitsRateInTransmiton BitRateInTransmition, out ACR122U_StatusModulationType ModulationType)
        {
            bool HasCard;
            byte[] Data;

            byte[] CommandAsBytes = new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x02, 0xD4, 0x04 };

            Context.Control(CommandAsBytes, out Data, out HasCard);

            ErrorCode = (ACR122U_StatusErrorCodes)Data[2];
            FieldPresent = Data[3] != 0;
            NumberOfTargets = Data[4];
            //required as data length doesnt change but some data is droped while leaving trailing 0s to force length of the same
            //if a card is returned with a card this is the values 
            if (Data.Length == 12 && Data[9] == 0x80 && Data[10] == 0x90 && Data[11] == 0x00)
            {
                Card = true;
                LogicalNumber = Data[5];
                BitRateInReception = (ACR122U_StatusBitRateInReception)Data[6];
                BitRateInTransmition = (ACR122U_StatusBitsRateInTransmiton)Data[7];
                ModulationType = (ACR122U_StatusModulationType)Data[8];

                return ACR122U_ResposeErrorCodes.Success;
            }
            else if (Data.Length == 8 && Data[5] == 0x80 && Data[6] == 0x90 && Data[7] == 0x00)
            {
                Card = false;
                LogicalNumber = 0;
                BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
                BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
                ModulationType = ACR122U_StatusModulationType.NoCardDetected;
                return ACR122U_ResposeErrorCodes.Success;
            }

            throw new ACR122U_SmartCardException(ACR122U_ResposeErrorCodes.APIError, ErrorCodes.SCARD_S_SUCCESS, "API has recived a unexpected value back.");
        }

        /// <summary>
        /// Gets the Status from the ACR122 using its internal method
        /// </summary>
        /// <param name="ACR122U_Status">A container with all of the status</param>
        /// <returns></returns>
        public static ACR122U_ResposeErrorCodes GetStatusStatic(WinSmartCardContext Context, out ACR122U_MifareClassic_Status ACR122U_Status)
        {
            bool Card;
            ACR122U_StatusErrorCodes ErrorCode;
            bool FieldPresent;
            byte NumberOfTargets;
            byte LogicalNumber;
            ACR122U_StatusBitRateInReception BitRateInReception;
            ACR122U_StatusBitsRateInTransmiton BitRateInTransmition;
            ACR122U_StatusModulationType ModulationType;
            ACR122U_ResposeErrorCodes Error = GetStatusStatic(Context, out Card, out ErrorCode, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitRateInTransmition, out ModulationType);
            ACR122U_Status = new ACR122U_MifareClassic_Status(Card, ErrorCode, FieldPresent, NumberOfTargets, LogicalNumber, BitRateInReception, BitRateInTransmition, ModulationType);
            return Error;
        }
        #endregion

        #region Deperecated
        //ACS has deperecated both of these commands in the newer API 
        /*Turn On/Off anntenna (Couldnt Figure out what this was actually doing out actually)
        *Send 
        FF 00 00 00 04 D4 [000<?:1=on,0=off>]
        *Returns
        D5 33 90 00 for success(90 00)
        */
        ///// <summary>
        ///// Turns RFID anntenna On
        ///// </summary>
        ///// <returns></returns>
        //public ACR122U_ResposeErrorCodes TurnAnntennaOn()
        //{
        //    byte[] DataOut;

        //    byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x01 };

        //    TransmitData(CommandAsBytes, out DataOut);

        //    LastACRResultCode = RetrieveDataCodes(ref DataOut);
        //    return LastACRResultCode;
        //}

        //*Turn On/Off anntenna (Couldnt Figure out what this was actually doing out actually)
        //*Send 
        //FF 00 00 00 04 D4 [000<?:1=on,0=off>]
        //*Returns
        //D5 33 90 00 for success(90 00)
        //*/
        ///// <summary>
        ///// Turns RFID anntenna off
        ///// </summary>
        ///// <returns></returns>
        //#warning You have re-enable a depreciated funtion TurnAnntennaOff that hardward no longer supports
        //[Obsolete]
        //public ACR122U_ResposeErrorCodes TurnAnntennaOff()
        //{
        //    byte[] DataOut;

        //    byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x00 };

        //    TransmitData(CommandAsBytes, out DataOut);

        //    LastACRResultCode = RetrieveDataCodes(ref DataOut);
        //    return LastACRResultCode;
        //}
        #endregion

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

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0x00, 0x50, 0x00, 0x00 };

            TransmitData(CommandAsBytes, out Data);

            LastACRResultCode = RetrieveDataCodes(ref Data, out DataOut);
            return LastACRResultCode;
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

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0x00, 0x51, SetInDataOut, 0x00 };

            TransmitData(CommandAsBytes, out Data);

            LastACRResultCode = RetrieveDataCodes(ref Data, out SetInDataOut);
            return LastACRResultCode;
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
            if (!Enum.IsDefined(typeof(ACR122U_BuzzerControl), BuzzerControl))
                throw new Exception("Your BuzzerControl selection was not valid.");

            byte[] Data;

            byte[] CommandAsBytes = new byte[8] { 0xFF, 0x00, 0x40, (byte)LEDControl, 0x04, T1Duration, T1Duration, (byte)BuzzerControl };

            TransmitData(CommandAsBytes, out Data);

            LastACRResultCode = RetrieveDataCodes(ref Data, out DataOut);
            return LastACRResultCode;
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

            byte[] CommandAsBytes = new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x02, 0xD4, 0x04 }; 

            TransmitData(CommandAsBytes, out Data);

            ErrorCode = (ACR122U_StatusErrorCodes)Data[2];
            FieldPresent = Data[3] != 0;
            NumberOfTargets = Data[4];
            if (Data.Length == 12 && Data[9]== 0x80 && Data[10] == 0x90 && Data[11] == 0x00)
            {
                Card = true;
                LogicalNumber = Data[5];
                BitRateInReception = (ACR122U_StatusBitRateInReception)Data[6];
                BitRateInTransmition = (ACR122U_StatusBitsRateInTransmiton)Data[7];
                ModulationType = (ACR122U_StatusModulationType)Data[8];

                LastACRResultCode = ACR122U_ResposeErrorCodes.Success;
                return LastACRResultCode;
            }
            else if (Data.Length == 8 && Data[5] == 0x80 && Data[6] == 0x90 && Data[7] == 0x00)
            {
                Card = false;
                LogicalNumber = 0;
                BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
                BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
                ModulationType = ACR122U_StatusModulationType.NoCardDetected;

                LastACRResultCode = ACR122U_ResposeErrorCodes.Success;
                return LastACRResultCode;
            }

            throw new ACR122U_SmartCardException(ACR122U_ResposeErrorCodes.APIError, ErrorCodes.SCARD_S_SUCCESS, "API has recived a unexpected value back.");
        }

        /// <summary>
        /// Gets the Status from the ACR122 using its internal method
        /// </summary>
        /// <param name="ACR122U_Status">A container with all of the status</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes GetStatus(out ACR122U_MifareClassic_Status ACR122U_Status)
        {
            bool Card;
            ACR122U_StatusErrorCodes ErrorCode;
            bool FieldPresent;
            byte NumberOfTargets;
            byte LogicalNumber;
            ACR122U_StatusBitRateInReception BitRateInReception;
            ACR122U_StatusBitsRateInTransmiton BitRateInTransmition;
            ACR122U_StatusModulationType ModulationType;
            ACR122U_ResposeErrorCodes Error = GetStatus(out Card, out ErrorCode, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitRateInTransmition, out ModulationType);
            ACR122U_Status = new ACR122U_MifareClassic_Status(Card, ErrorCode, FieldPresent, NumberOfTargets, LogicalNumber, BitRateInReception, BitRateInTransmition, ModulationType);
            return Error;
        }

        #endregion
        
        //not 100% sure this section is need but Ill throw it here as taken from a second dialog after identification
        #region ISO1443OrMifareCardCommands

        #endregion


    }
}
