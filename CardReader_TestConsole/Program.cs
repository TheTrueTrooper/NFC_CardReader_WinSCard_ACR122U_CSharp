using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFC_CardReader;
using NFC_CardReader.ACR122U;

namespace CardReader_TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region WincardAPIImportTesting

            #region ContextInIt
            WinSmartCardContext Context = new WinSmartCardContext(OperationScopes.SCARD_SCOPE_SYSTEM);
            #endregion

            #region ContextGetReaders
            Console.WriteLine("Currently connected readers: ");
            int MaxNumber = 0;
            List<string> Names = Context.ListReadersAsStrings();
            foreach (string Reader in Names)
            {
                Console.WriteLine("\t" + MaxNumber + ":" + Reader);
                MaxNumber += 1;
            }
            #endregion

            #region ContextConnect
            int Selection = 0;
            Console.WriteLine("Please try and select one.");
            while (!(int.TryParse(Console.ReadLine(), out Selection) && -1 < Selection && Selection < MaxNumber))
            {
                Console.WriteLine("Oops thats not a valid selection number. Try again.");
            }
            Console.WriteLine("Testing polling/blocking calls");
            Console.WriteLine("Please change state");
            ReadersCurrentState[] States = new ReadersCurrentState[] { new ReadersCurrentState() { ReaderName = Names[Selection] } };
            Context.GetStatusChange(100000, ref States);
            Console.WriteLine("Test 1 Results");
            Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
            Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
            Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
            Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

            States[0].CurrentState = States[0].EventState;
            Context.GetStatusChange(100000, ref States);
            Console.WriteLine("Test 2 Results");
            Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
            Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
            Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
            Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

            States[0].CurrentState = States[0].EventState;
            Context.GetStatusChange(100000, ref States);
            Console.WriteLine("Test 3 Results");
            Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
            Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
            Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
            Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

            WinSmartCard WSC = Context.Connect(Names[Selection], SmartCardShareTypes.SCARD_SHARE_SHARED);
            Console.WriteLine("Connected to card as winscard.\n\tProperties are");

            Console.WriteLine("\t\tIsAliveWithAContext: " + WSC.IsAliveWithContext);
            Console.WriteLine("\t\tATRString: " + WSC.ATRString);
            Console.WriteLine("\t\tATR(ConvertedFromBytes): " + BitConverter.ToString(WSC.ATR.ToArray()));
            Console.WriteLine("\t\tProtocol: " + WSC.Protocol);
            Console.WriteLine("\t\tReaderName: " + WSC.Parent.ConnectedReaderName);
            #endregion

            #region WinscardGetStatus
            SmartCardStatus Status;
            SmartCardProtocols Protocol;
            string ATR;
            string ReaderName;
            WSC.GetStatus(out ReaderName, out Status, out Protocol, out ATR);
            Console.WriteLine("Getting Status");
            Console.WriteLine("\tReaderName: " + ReaderName);
            Console.WriteLine("\tStatus: " + Status);
            Console.WriteLine("\tProtocol: " + Protocol);
            Console.WriteLine("\tATR: " + ATR);
            #endregion

            #region WinscardGetATR(NotSupportedByMyReader)
            WSC.GetAttrib((SmartCardATR)400100, out ATR);
            Console.WriteLine("Getting ATR");
            Console.WriteLine("\tATR: Get ATR is not supported by this device");
            #endregion

            #endregion

            #region ACR122U_ADU_API_Testing

            #region InIt
            ACR122U_SmartCard ACR122U_SmartCard = new ACR122U_SmartCard(WSC);
            Console.WriteLine("Upgraded connection to card to ACR122U_SmartCard for API");
            #endregion

            #region ACRGetStatus
            bool CardPresent;
            ACR122U_StatusErrorCodes ACRError;
            bool FieldPresent;
            byte NumberOfTargets;
            byte LogicalNumber;
            ACR122U_StatusBitRateInReception BitRateInReception;
            ACR122U_StatusBitsRateInTransmiton BitsRateInTransmiton;
            ACR122U_StatusModulationType ModulationType;
            ACR122U_SmartCard.GetStatus(out CardPresent, out ACRError, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitsRateInTransmiton, out ModulationType);
            Console.WriteLine("Getting ACR122u Status");
            Console.WriteLine("\tCard Present: " + CardPresent);
            Console.WriteLine("\tACR Error: " + ACRError);
            Console.WriteLine("\tFields Present: " + FieldPresent);
            Console.WriteLine("\tNumber Of Targets: " + NumberOfTargets);
            Console.WriteLine("\tLogical Number: " + LogicalNumber);
            Console.WriteLine("\tBit Rate In Reception: " + BitRateInReception);
            Console.WriteLine("\tBit Rate In Transmiton: " + BitsRateInTransmiton);
            Console.WriteLine("\tModulation Type: " + ModulationType);
            #endregion

            #region ACRGet/SetPICC
            ACR122U_PICCOperatingParametersControl Settings;
            ACR122U_SmartCard.GetPICCOperatingParameterState(out Settings);
            Console.WriteLine("Getting PICC: " + Settings);
            Settings = ACR122U_PICCOperatingParametersControl.AllOff;
            ACR122U_SmartCard.SetPICCOperatingParameterState(ref Settings);
            Console.WriteLine("Setting PICC");
            Console.WriteLine("\tPICC Setting Return: " + Settings);
            ACR122U_SmartCard.GetPICCOperatingParameterState(out Settings);
            Console.WriteLine("Getting PICC: " + Settings);
            Settings = ACR122U_PICCOperatingParametersControl.AllOn;
            ACR122U_SmartCard.SetPICCOperatingParameterState(ref Settings);
            Console.WriteLine("Setting PICC");
            Console.WriteLine("\tPICC Setting Return: " + Settings);
            #endregion

            #region GetUDI
            Console.WriteLine("UDI string: " + ACR122U_SmartCard.GetcardUID());
            #endregion

            #region GetATS(NotSupportedByMyParticACRorItsFirmware)
            try
            {
                Console.WriteLine("ATS string: " + ACR122U_SmartCard.GetcardATS());
            }
            catch (ACR122U_SmartCardException Ex)
            {
                if (Ex.ACRErrorOnException == ACR122U_ResposeErrorCodes.FuctionNotSupported)
                    Console.WriteLine("ATS string: " + ACR122U_SmartCard.GetACRErrMsg(Ex.ACRErrorOnException));
                else
                    throw Ex;
            }
            #endregion

            #region SetLEDandBuzzerControl
            Console.WriteLine("Testing LED/BuzzerControl\n\tUnit shoud T1 & T2 Buzz for 2000ms. This should happen 2 times with blinks between.\n\tPay close attention with the break points first time through.\n\tAfter it is a physical test.");
            byte OddData;
            ACR122U_SmartCard.SetLEDandBuzzerControl(ACR122U_LEDControl.InitialRedBlinkingState | ACR122U_LEDControl.RedBlinkingMask | ACR122U_LEDControl.RedLEDStateMask | ACR122U_LEDControl.GreenFinalState, 20, 20, 2, ACR122U_BuzzerControl.BuzzerOnT1Cycle, out OddData);
            Console.WriteLine("\tDone.\n\tAdditional odd Data(some times shows with no expanation): " + OddData);
            #endregion

            #endregion

            Console.ReadKey();
        }
    }
}
