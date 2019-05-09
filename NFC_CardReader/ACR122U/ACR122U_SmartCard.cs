using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        static ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data)
        {
            byte[] OpReturn = new byte[4] { Data[Data.Length - 1], Data[Data.Length - 2], 0, 0 };
            Array.Resize(ref Data, Data.Length - 2);
            return (ACR122U_ResposeErrorCodes)BitConverter.ToInt32(OpReturn, 0);
        }
        //0xd5 0x41 0x27
        static ACR122U_ResposeErrorCodes RetrieveDataCodes(ref byte[] Data, out byte TrailingResposeData)
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
        /// Turns RFID anntenna On with out need of card
        /// </summary>
        /// <returns></returns>
        public static ACR122U_ResposeErrorCodes TurnAnntennaOnStatic(WinSmartCardContext Context)
        {
            bool HasCard;
            byte[] DataOut;

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x01 };

            Context.Control(CommandAsBytes, out DataOut, out HasCard);

            return RetrieveDataCodes(ref DataOut);
        }

        /// <summary>
        /// Turns RFID anntenna off with out need of card
        /// </summary>
        /// <returns></returns>
        public static ACR122U_ResposeErrorCodes TurnAnntennaOffStatic(WinSmartCardContext Context)
        {
            bool HasCard;
            byte[] DataOut;

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x00 };

            Context.Control(CommandAsBytes, out DataOut, out HasCard);

            return RetrieveDataCodes(ref DataOut);
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
            if (Data[9] == 0x80 && Data[10] == 0x90 && Data[11] == 0x00)
            {
                Card = true;
                LogicalNumber = Data[5];
                BitRateInReception = (ACR122U_StatusBitRateInReception)Data[6];
                BitRateInTransmition = (ACR122U_StatusBitsRateInTransmiton)Data[7];
                ModulationType = (ACR122U_StatusModulationType)Data[8];

                return ACR122U_ResposeErrorCodes.Success;
            }
            //if a card is returned with out card this is the values
            else if (Data[5] == 0x80 && Data[6] == 0x90 && Data[7] == 0x00)
            {
                Card = false;
                LogicalNumber = 0;
                BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
                BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
                ModulationType = ACR122U_StatusModulationType.NoCardDetected;

                return ACR122U_ResposeErrorCodes.Success;
            }
            else
                throw new ACR122U_SmartCardException(ACR122U_ResposeErrorCodes.APIError, ErrorCodes.SCARD_S_SUCCESS);
        }
        #endregion

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

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x01 };

            TransmitData(CommandAsBytes, out DataOut);

            LastACRResultCode = RetrieveDataCodes(ref DataOut);
            return LastACRResultCode;
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

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0x00, 0x00, 0x00, 0x04, 0xD4, 0x00 };

            TransmitData(CommandAsBytes, out DataOut);

            LastACRResultCode = RetrieveDataCodes(ref DataOut);
            return LastACRResultCode;
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

                LastACRResultCode = ACR122U_ResposeErrorCodes.Success;
                return LastACRResultCode;
            }

            Card = false;
            LogicalNumber = 0;
            BitRateInReception = ACR122U_StatusBitRateInReception.NoReception;
            BitRateInTransmition = ACR122U_StatusBitsRateInTransmiton.NoTransmiton;
            ModulationType = ACR122U_StatusModulationType.NoCardDetected;
            LastACRResultCode = ACR122U_ResposeErrorCodes.Success;
            return LastACRResultCode;
        }

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
            byte[] CommandAsBytes = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 };

            TransmitData(CommandAsBytes, out receivedUID);

            LastACRResultCode = RetrieveDataCodes(ref receivedUID);
            return LastACRResultCode;
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

            cardUID = BitConverter.ToString(receivedUID);

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

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xCA, 0x01, 0x00, 0x00 };

            TransmitData(CommandAsBytes, out receivedUID);

            LastACRResultCode = RetrieveDataCodes(ref receivedUID);
            return LastACRResultCode;
        }

        /// <summary>
        /// Gets The ATS Current particuar hardware or firmware or card doesnt support
        /// </summary>
        /// <param name="receivedUID"></param>
        /// <returns></returns>
        public string GetcardATS()//only for mifare 1k cards
        {

            string cardATS = "";
            byte[] receivedUID;

            LastACRResultCode = GetcardATSBytes(out receivedUID);

            if (LastACRResultCode != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(LastACRResultCode, LastResultCode);

            cardATS = BitConverter.ToString(receivedUID);  

            return cardATS;
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
            if (!Enum.IsDefined(typeof(ACR122U_KeyMemories), Key))
                throw new Exception("Your Key Load location is not a valid one.");
            if (KeyValue.Length != 6)
                throw new Exception("Your Key has too many or too few bites.\nKeys must have 6 bytes.");

            byte[] CommandAsBytes = new byte[11] { 0xFF, 0x82, 0x00, (byte)Key, 0x06, KeyValue[0], KeyValue[1], KeyValue[2], KeyValue[3], KeyValue[4], KeyValue[5] };

            TransmitData(CommandAsBytes, out Return);

            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <param name="KeyValue">The Value Key to use. Length must be six</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes LoadAthenticationKeys(ACR122U_Keys Key, byte[] KeyValue)
        {
            if(!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection is not a valid one.");

            return LoadAthenticationKeys((ACR122U_KeyMemories)Key, KeyValue);
        }

        /*Load Key
        *Send 
        FF 86 00 00 05 01 00 00 60 01
        or
        FF 86 00 00 05 01 00 [Block:00-FF] 6{000<KeyType:0=A,1=B>} [KeyNumber:00-01]
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
            if (!Enum.IsDefined(typeof(ACR122U_KeyMemories), KeyToUse))
                throw new Exception("Your Key Load location is not a valid one.");
            if (!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection is not a valid one.");

            byte[] Return;
            byte[] CommandAsBytes = new byte[10] { 0xFF, 0x86, 0x00, 0x00, 0x05, 0x01, 0x00, BlockToAthenticate, (byte)(0x60 | (byte)Key), (byte)KeyToUse }; 
            
            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_Keys KeyToUse)
        {
            if (!Enum.IsDefined(typeof(ACR122U_Keys), KeyToUse))
                throw new Exception("Your Key Selection for KeyToUse is not a valid one.");
            if (!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection for Key is not a valid one.");
            LastACRResultCode = Athentication( BlockToAthenticate, Key, (ACR122U_KeyMemories)KeyToUse);
            return LastACRResultCode;
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

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xB0, 0x00, BlockToRead, NumberToRead };

            TransmitData(CommandAsBytes, out DataOut);

            LastACRResultCode = RetrieveDataCodes(ref DataOut);
            return LastACRResultCode;
        }

        /*WriteBlock
        *Send 
        FF D6 00 51 10 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
        or
        FF D6 00 51 10 [Data]...
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

            byte[] CommandAsBytes = new byte[21] { 0xFF, 0xD6, 0x00, BlockToWrite, 0x10, DataIn[0], DataIn[1], DataIn[2], DataIn[3], DataIn[4], DataIn[5], DataIn[6], DataIn[7], DataIn[8], DataIn[9], DataIn[10], DataIn[11], DataIn[12], DataIn[13], DataIn[14], DataIn[15] }; 

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
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
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">The number output</param>
        /// <param name="BlockToRead">The Block to Read from</param>
        /// <returns></returns>
        // dont believe the compiler it lies my int is bigger.....
        public ACR122U_ResposeErrorCodes ReadValueFromBlock(out Int32 Value, byte BlockToRead)
        {
            ACR122U_ResposeErrorCodes Return;
            byte[] DataBack;

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xB1, 0x00, BlockToRead, 0x04 };

            TransmitData(CommandAsBytes, out DataBack);
            Return = RetrieveDataCodes(ref DataBack);
            if (Return == ACR122U_ResposeErrorCodes.Success)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(DataBack);
                Value = BitConverter.ToInt32(DataBack, 0);
            }
            else
                Value = 0;
            LastACRResultCode = Return;
            return LastACRResultCode;
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
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">the number to write</param>
        /// <param name="BlockToWrite">The Block to write to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes WriteValueToBlock(Int32 Value, byte BlockToWrite)
        {
            byte[] Return;
            byte[] ValuesBytes;
            ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToWrite, 0x05, 0x00, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] };//BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
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
        public ACR122U_ResposeErrorCodes IncrementValue(Int32 Value, byte BlockToIncrement)
        {
            byte[] Return;
            byte[] ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToIncrement, 0x05, 0x01, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] }; //BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
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
        public ACR122U_ResposeErrorCodes DecrementValue(Int32 Value, byte BlockToDecrement)
        {
            byte[] Return;
            byte[] ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToDecrement, 0x05, 0x02, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] };

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
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

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0xD7, 0x00, SourceBlock, 0x02, 0x03, TargetBlock };//BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        #endregion
        //not 100% sure this section is need but Ill throw it here as taken from a second dialog after identification
        #region ISO1443OrMifareCardCommands

        #endregion


    }
}
