using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NFC_CardReader;
using NFC_CardReader.ACR122U;
using NFC_CardReader.ACR122U.CardTypes.MifareClassic;
using NFC_CardReader.ACR122UManager;
using System.IO;
using NFC_CardReader.WinSCard;

#region BaseTesting
//#region WincardAPIImportTesting

//#region ContextGetReaders
//Console.WriteLine("Currently connected readers: ");
//int MaxNumber = 0;
//List<string> Names = WinSmartCardContext.ListReadersAsStringsStatic();
//foreach (string Reader in Names)
//{
//    Console.WriteLine("\t" + MaxNumber + ":" + Reader);
//    MaxNumber += 1;
//}
//Console.WriteLine("Filtering to usable test readers. Note: driver update adds ACS ACR122U PICC Interface that are not usable.");
//Console.WriteLine("Currently connected ACR122U readers: ");
//Names = WinSmartCardContext.ListReadersAsStringsStatic();
//Names = Names.Where(x=> x.Contains("ACS ACR122") && !x.Contains("ACS ACR122U PICC Interface")).ToList();
//MaxNumber = 0;
//foreach (string Reader in Names)
//{
//    Console.WriteLine("\t" + MaxNumber + ":" + Reader);
//    MaxNumber += 1;
//}
//#endregion

//#region ContextConnect
//int Selection = 0;
////Console.WriteLine("Please try and select one.\nNote do not pick ACS ACR122U PICC Interface.\nDriver update added them and it doesnt work.\nA ACS ACR122, will work however.");
//Console.WriteLine("Please try and select one.");
//while (!(int.TryParse(Console.ReadLine(), out Selection) && -1 < Selection && Selection < MaxNumber))
//{
//    Console.WriteLine("Oops thats not a valid selection number. Try again.");
//    //"ACS ACR122U PICC Interface Interface 0"
//}

//#region ContextInIt
//WinSmartCardContext Context = new WinSmartCardContext(OperationScopes.SCARD_SCOPE_SYSTEM, Names[Selection]);

//#region CardLessStaticFuncs/Methods
//ACR122U_PICCOperatingParametersControl Settings;
//NFC_CardReader.ACR122U.ACR122U_SmartCard.GetPICCOperatingParameterStateStatic(Context, out Settings);
//Console.WriteLine("Getting PICC: " + Settings);
//#endregion

//#endregion

//#region WinscardStatusChange
//Console.WriteLine("Testing polling/blocking calls");
//Console.WriteLine("Please change state");
//ReadersCurrentState[] States = new ReadersCurrentState[] { new ReadersCurrentState() { ReaderName = Names[Selection] } };
//Context.GetStatusChange(5000, ref States);
//Console.WriteLine("Test 1 Results (Init)");
//Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
//Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
//Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
//Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

//States[0].CurrentState = States[0].EventState;
//Context.GetStatusChange(5000, ref States);
//Console.WriteLine("Test 2 Results (with Timeout)");
//Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
//Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
//Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
//Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

//States[0].CurrentState = States[0].EventState;
//Context.GetStatusChange(5000, ref States);
//Console.WriteLine("Test 3 Results (with Timeout)");
//Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
//Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
//Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
//Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

//ReadersCurrentState LastState;

/////Again but this time for ever
//States[0].CurrentState = States[0].EventState;
//LastState = States[0];
//while (LastState.EventState == States[0].EventState)
//{
//    Context.GetStatusChange(5000, ref States);
//}
//Console.WriteLine("Test 4 Results (forever)");
//Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
//Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
//Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
//Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);


//States[0].CurrentState = States[0].EventState;
//LastState = States[0];
//while (LastState.EventState == States[0].EventState)
//{
//    Context.GetStatusChange(5000, ref States);
//}
//Console.WriteLine("Test 5 Results (forever)");
//Console.WriteLine("\t\tStates ATR: " + BitConverter.ToString(States.Last().ATR));
//Console.WriteLine("\t\tStates Event State: " + States[0].EventState);
//Console.WriteLine("\t\tStates Current State: " + States[0].CurrentState);
//Console.WriteLine("\t\tStates Changed Reader: " + States[0].ReaderName);

//#endregion

//#endregion

//WinSmartCard WSC = Context.CardConnect(SmartCardShareTypes.SCARD_SHARE_SHARED);
//Console.WriteLine("Connected to card as winscard.\n\tProperties are");

//Console.WriteLine("\t\tIsAliveWithAContext: " + WSC.IsAliveWithContext);
//Console.WriteLine("\t\tATRString: " + WSC.ATRString);
//Console.WriteLine("\t\tATR(ConvertedFromBytes): " + BitConverter.ToString(WSC.ATR.ToArray()));
//Console.WriteLine("\t\tProtocol: " + WSC.Protocol);
//Console.WriteLine("\t\tReaderName: " + WSC.Parent.ConnectedReaderName);

//#region WinscardGetStatus
//SmartCardStatus Status;
//SmartCardProtocols Protocol;
//string ATR;
//string ReaderName;
//WSC.GetStatus(out ReaderName, out Status, out Protocol, out ATR);
//Console.WriteLine("Getting Status");
//Console.WriteLine("\tReaderName: " + ReaderName);
//Console.WriteLine("\tStatus: " + Status);
//Console.WriteLine("\tProtocol: " + Protocol);
//Console.WriteLine("\tATR: " + ATR);
//#endregion

//#region WinscardGetATR(NotSupportedByMyReader)
////WSC.GetAttrib((SmartCardATR)400100, out ATR);
//Console.WriteLine("Getting ATR");
//Console.WriteLine("\tATR: Get ATR is not supported by this device");
//#endregion

//#endregion

//#region ACR122U_ADU_API_Testing

//#region InIt
//ACR122U_SmartCard ACR122U_SmartCard = new ACR122U_SmartCard(WSC);
//Console.WriteLine("Upgraded connection to card to ACR122U_SmartCard for API");
//#endregion

//#region ACRGetStatus
//bool CardPresent;
//ACR122U_StatusErrorCodes ACRError;
//bool FieldPresent;
//byte NumberOfTargets;
//byte LogicalNumber;
//ACR122U_StatusBitRateInReception BitRateInReception;
//ACR122U_StatusBitsRateInTransmiton BitsRateInTransmiton;
//ACR122U_StatusModulationType ModulationType;
//ACR122U_SmartCard.GetStatus(out CardPresent, out ACRError, out FieldPresent, out NumberOfTargets, out LogicalNumber, out BitRateInReception, out BitsRateInTransmiton, out ModulationType);
//Console.WriteLine("Getting ACR122u Status");
//Console.WriteLine("\tCard Present: " + CardPresent);
//Console.WriteLine("\tACR Error: " + ACRError);
//Console.WriteLine("\tFields Present: " + FieldPresent);
//Console.WriteLine("\tNumber Of Targets: " + NumberOfTargets);
//Console.WriteLine("\tLogical Number: " + LogicalNumber);
//Console.WriteLine("\tBit Rate In Reception: " + BitRateInReception);
//Console.WriteLine("\tBit Rate In Transmiton: " + BitsRateInTransmiton);
//Console.WriteLine("\tModulation Type: " + ModulationType);
////PrintACRError(ACR122U_SmartCard);
//#endregion

//#region ACRGet/SetPICC
//ACR122U_SmartCard.GetPICCOperatingParameterState(out Settings);
//Console.WriteLine("Getting PICC: " + Settings);
//Settings = ACR122U_PICCOperatingParametersControl.AllOff;
//ACR122U_SmartCard.SetPICCOperatingParameterState(ref Settings);
//Console.WriteLine("Setting PICC");
//Console.WriteLine("\tPICC Setting Return: " + Settings);
//ACR122U_SmartCard.GetPICCOperatingParameterState(out Settings);
//Console.WriteLine("Getting PICC: " + Settings);
//Settings = ACR122U_PICCOperatingParametersControl.AllOn;
//ACR122U_SmartCard.SetPICCOperatingParameterState(ref Settings);
//Console.WriteLine("Setting PICC");
//Console.WriteLine("\tPICC Setting Return: " + Settings);
//ACR122U_SmartCard.GetPICCOperatingParameterState(out Settings);
//Console.WriteLine("Getting PICC: " + Settings);
////PrintACRError(ACR122U_SmartCard);
//#endregion

//#region GetUDI
//Console.WriteLine("UDI string: " + ACR122U_SmartCard.GetcardUID());
//#endregion

//#region GetATS(NotSupportedByMyParticACRorItsFirmware)
//try
//{
//    Console.WriteLine("ATS string: " + ACR122U_SmartCard.GetcardATS());
//}
//catch (ACR122U_SmartCardException Ex)
//{
//    if (Ex.ACRErrorOnException == ACR122U_ResposeErrorCodes.FuctionNotSupported)
//        Console.WriteLine("ATS string: " + ACR122U_SmartCard.GetACRErrMsg(Ex.ACRErrorOnException));
//    else
//        throw Ex;
//}
//PrintACRError(ACR122U_SmartCard);
//#endregion

//#region LoadAthenticationAndAthenticate+ReadWrite
//Console.WriteLine("Loading athentication Keys to 1: " + ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));

//byte[] Data;
//Console.WriteLine("Attempting to write block 4 (sector 1, block 1) expected fail(not athenticated): " + ACR122U_SmartCard.WriteBlock(new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5));
//Console.WriteLine("Attempting to read block 4 (sector 1, block 1) expected fail(not athenticated): " + ACR122U_SmartCard.ReadBlock(out Data,  5));
////PrintACRError(ACR122U_SmartCard);

//Console.WriteLine("Athentication Key A to 1: " + ACR122U_SmartCard.Athentication(5, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
////PrintACRError(ACR122U_SmartCard);
////seems to return true if keys are the same?
//Console.WriteLine("Attempting to write block 5 (sector 1, block 1) expected fail(not athenticated(A is read)): " + ACR122U_SmartCard.WriteBlock(new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5));
//Console.WriteLine("Attempting to read block 5 (sector 1, block 1): " + ACR122U_SmartCard.ReadBlock(out Data, 5));
//Console.WriteLine("\tData: " + BitConverter.ToString(Data));
////PrintACRError(ACR122U_SmartCard);
//Console.WriteLine("Athentication Key B to 1: " + ACR122U_SmartCard.Athentication(5, ACR122U_Keys.KeyB, ACR122U_KeyMemories.Key1));

//Console.WriteLine("Attempting to write block 5 (sector 1, block 1) All 0xFF: " + ACR122U_SmartCard.WriteBlock(new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5));
//Console.WriteLine("Attempting to read block 5 (sector 1, block 1): " + ACR122U_SmartCard.ReadBlock(out Data, 5));
//Console.WriteLine("\tData: " + BitConverter.ToString(Data));

//Console.WriteLine("Attempting to write block 5 (sector 1, block 1) All 0x00: " + ACR122U_SmartCard.WriteBlock(new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 5));
//Console.WriteLine("Attempting to read block 5 (sector 1, block 1): " + ACR122U_SmartCard.ReadBlock(out Data, 5));
//Console.WriteLine("\tData: " + BitConverter.ToString(Data));
//#endregion

//#region Values
//Int32 Data2;
//Console.WriteLine("Attempting to write value to block 5 (sector 1, block 1) Value = 5: " + ACR122U_SmartCard.WriteValueToBlock(5, 5));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value ?= 5: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);

//Console.WriteLine("Attempting to write value to block 5 (sector 1, block 1) Value = 0: " + ACR122U_SmartCard.WriteValueToBlock(0, 5));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value ?= 0: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);

//Console.WriteLine("Attempting to increment value at block 5 (sector 1, block 1): " + ACR122U_SmartCard.IncrementValue(1, 5));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value ?= 1: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);

//Console.WriteLine("Attempting to decrement value at block 5 block 5 (sector 1, block 1): " + ACR122U_SmartCard.DecrementValue(1, 5));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value ?= 0: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);

//Console.WriteLine("Attempting to increment value at block 5 (sector 1, block 1): " + ACR122U_SmartCard.IncrementValue(1, 5));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value ?= 1: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);

//Console.WriteLine("Attempting to copy value at block 5 to block 4 (sector 1, block 1 => sector 1, block 0): " + ACR122U_SmartCard.Copy(5, 4));
//Console.WriteLine("Attempting to read value from block 5 (sector 1, block 1) Value: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 5));
//Console.WriteLine("\tData: " + Data2);
//Console.WriteLine("Attempting to read value from block 4 (sector 1, block 1) Value[4] ?= Value[5]: " + ACR122U_SmartCard.ReadValueFromBlock(out Data2, 4));
//Console.WriteLine("\tData: " + Data2);

//#endregion

//#region SetLEDandBuzzerControl
////Console.WriteLine("Testing LED/BuzzerControl\n\tUnit shoud T1 & T2 Buzz for 2000ms. This should happen 2 times with blinks between.\n\tPay close attention with the break points first time through.\n\tAfter it is a physical test.");
////byte OddData;
////ACR122U_SmartCard.SetLEDandBuzzerControl(ACR122U_LEDControl.InitialRedBlinkingState | ACR122U_LEDControl.RedBlinkingMask | ACR122U_LEDControl.RedLEDStateMask | ACR122U_LEDControl.GreenFinalState, 20, 20, 2, ACR122U_BuzzerControl.BuzzerOnT1Cycle, out OddData);
////Console.WriteLine("\tDone.\n\tAdditional odd Data(some times shows with no expanation): " + OddData);
//#endregion

//#region CardLessStaticFuncs/Methods
////NFC_CardReader.ACR122U.ACR122U_SmartCard.GetPICCOperatingParameterStateStatic(Context, out Settings);
////Console.WriteLine("Getting PICC: " + Settings);
//#endregion

////ACR122U_SmartCard.Dispose();
////Context.Dispose();
//#endregion
//Console.WriteLine("Athentication Key A to 1: " + ACR122U_SmartCard.Athentication(5, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));

//Console.ReadKey();
#endregion
#region NFCMaifareClassicTesting
//namespace CardReader_TestFileLogger
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            byte[] AcceptedATR = new byte[] { 0x3B, 0x8F, 0x80, 0x01, 0x80, 0x4F, 0x0C, 0xA0, 0x00, 0x00, 0x03, 0x06, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6A };
//            ACR122UManager Manager = new ACR122UManager(ACR122UManager.GetACR122UReaders().FirstOrDefault());
//            //
//            ACR122U_MifareClassic_Status Status;
//            Manager.GetStatus(out Status);
//            //
//            ACR122U_PICCOperatingParametersControl ControlOptions = ACR122U_PICCOperatingParametersControl.AllOn;
//            Manager.SetPICCOperatingParameterState(ref ControlOptions);
//            //
//            Console.WriteLine("PIC options:\n" + ControlOptions);
//            Console.WriteLine("Starting Status:\n\tCard: " + Status.Card + "\n\tError: " + Status.ErrorCode);
//            //
//            ACR122UManager.GlobalCardCheck = (e) =>
//            {
//                bool CeckSuccess = false;
//                if (e.ATR.Length == AcceptedATR.Length)
//                {
//                    CeckSuccess = true;
//                    for (int i = 0; i < e.ATR.Length; i++)
//                    {
//                        if (e.ATR[i] != AcceptedATR[i])
//                        {
//                            CeckSuccess = false;
//                            break;
//                        }
//                    }
//                }
//                return CeckSuccess;
//            };

//            Manager.CheckCard = true;

//            ManagerTest Test = new ManagerTest(Manager);

//            Manager.AcceptedCardScaned += Test.TestAccept;
//            Manager.CardStateChanged += Test.TestStateChange;
//            Manager.RejectedCardScaned += Test.TestRejected;
//            Manager.CardDetected += Test.TestCardDetected;
//            Manager.CardRemoved += Test.TestCardRemoved;
//            List<string> Names = WinSmartCardContext.ListReadersAsStringsStatic();
//            Console.ReadKey();

//        }

//        static class FileLogger
//        {
//            static readonly string Location = Environment.CurrentDirectory + "\\CardReaderOutput.txt";

//            public static void WriteLine(string Write)
//            {
//                using (StreamWriter SW = new StreamWriter(File.Open(Location, FileMode.Append)))
//                {
//                    SW.WriteLine(Write);
//                }
//            }

//            public static void WriteLine(string Write, params object[] obj)
//            {
//                using (StreamWriter SW = new StreamWriter(File.Open(Location, FileMode.Append)))
//                {
//                    SW.WriteLine(string.Format(Write, obj));
//                }
//            }

//        }

//        public class ManagerTest
//        {

//            ACR122UManager Manager;

//            public ManagerTest(ACR122UManager M)
//            {
//                Manager = M;
//            }

//            public void TestStateChange(object sender, ACRCardStateChangeEventArg e)
//            {
//                Console.WriteLine("CardReaders state has changed");
//                Console.WriteLine("State Enum : {0}", e.EventState);
//                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
//                Console.WriteLine("ATR : {0}", e.ATRString);
//            }

//            public void TestAccept(object sender, ACRCardAcceptedCardScanEventArg e)
//            {
//                Console.WriteLine("CardReader has accepted Card");
//                Console.WriteLine("State Enum : {0}", e.EventState);
//                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
//                Console.WriteLine("ATR : {0}", e.ATRString);

//                if (Manager.Card == null)
//                {
//                    #region BasicConnect
//                    ACR122U_MifareClassic_SmartCard Card = Manager.ConnectToMifareClassicCard();
//                    Console.WriteLine("\tCard Conneted");
//                    Console.WriteLine("\tUDI: " + Card.GetcardUID());
//                    #endregion

//                    #region ValueTesting
//                    byte[] Data;
//                    Console.WriteLine("\tLoading athentication Keys to Key Memory 1: 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF");
//                    Card.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
//                    Console.WriteLine("\tAthentication Key B (Read/Write Key) to Key Memory 1: ");
//                    Card.Athentication(5, ACR122U_Keys.KeyB, ACR122U_KeyMemories.Key1);
//                    Console.WriteLine("\tAttempting to write block 5 (sector 1, block 1) All 0xFF: ");
//                    Card.WriteBlock(new byte[16] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5);
//                    Console.WriteLine("\tAttempting to read block 5 (sector 1, block 1): ");
//                    Card.ReadBlock(out Data, 5);
//                    Console.WriteLine("\tData: " + BitConverter.ToString(Data));
//                    Console.WriteLine("\tAttempting to write block 5 (sector 1, block 1) All 0x00: ");
//                    Card.WriteBlock(new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 5);
//                    Console.WriteLine("\tAttempting to read block 5 (sector 1, block 1): ");
//                    Card.ReadBlock(out Data, 5);
//                    Console.WriteLine("\tData: " + BitConverter.ToString(Data));
//                    #endregion

//                    #region Values
//                    Int32 Data2;
//                    Console.WriteLine("\tAttempting to write value to block 5 (sector 1, block 1) Value = 5: ");
//                    Card.WriteValueToBlock(5, 5);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value ?= 5: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to write value to block 5 (sector 1, block 1) Value = 0: ");
//                    Card.WriteValueToBlock(0, 5);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value ?= 0: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to increment value at block 5 (sector 1, block 1): ");
//                    Card.IncrementValue(1, 5);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value ?= 1: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to decrement value at block 5 block 5 (sector 1, block 1): ");
//                    Card.DecrementValue(1, 5);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value ?= 0: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to increment value at block 5 (sector 1, block 1): ");
//                    Card.IncrementValue(1, 5);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value ?= 1: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to copy value at block 5 to block 4 (sector 1, block 1 => sector 1, block 0): ");
//                    Card.Copy(5, 4);
//                    Console.WriteLine("\tAttempting to read value from block 5 (sector 1, block 1) Value: ");
//                    Card.ReadValueFromBlock(out Data2, 5);
//                    Console.WriteLine("\t\tData: " + Data2);

//                    Console.WriteLine("\tAttempting to read value from block 4 (sector 1, block 1) Value[4] ?= Value[5]: ");
//                    Card.ReadValueFromBlock(out Data2, 4);
//                    Console.WriteLine("\t\tData: " + Data2);
//                    #endregion

//                    Manager.DisconnectToCard();

//                }

//            }

//            public void TestRejected(object sender, ACRCardRejectedCardScanEventArg e)
//            {
//                Console.WriteLine("CardReader has rejected Card");
//                Console.WriteLine("State Enum : {0}", e.EventState);
//                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
//                Console.WriteLine("ATR : {0}", e.ATRString);
//            }

//            public void TestCardDetected(object sender, ACRCardDetectedEventArg e)
//            {
//                Console.WriteLine("CardReader has detected Card");
//                Console.WriteLine("State Enum : {0}", e.EventState);
//                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
//                Console.WriteLine("ATR : {0}", e.ATRString);
//            }

//            public void TestCardRemoved(object sender, ACRCardRemovedEventArg e)
//            {
//                Console.WriteLine("CardReader has removed Card");
//                Console.WriteLine("State Enum : {0}", e.EventState);
//                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
//                Console.WriteLine("ATR : {0}", e.ATRString);

//                //Manager.DisconnectToCard();
//            }
//        }

//    }
//}
#endregion
namespace CardReader_TestFileLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] AcceptedATR = new byte[] { 0x3B, 0x8F, 0x80, 0x01, 0x80, 0x4F, 0x0C, 0xA0, 0x00, 0x00, 0x03, 0x06, 0x03, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x68 };
            ACR122UManager Manager = new ACR122UManager(ACR122UManager.GetACR122UReaders().FirstOrDefault());
            //
            ACR122U_MifareClassic_Status Status;
            Manager.GetStatus(out Status);
            //
            ACR122U_PICCOperatingParametersControl ControlOptions = ACR122U_PICCOperatingParametersControl.AllOn;
            Manager.SetPICCOperatingParameterState(ref ControlOptions);
            //
            Console.WriteLine("PIC options:\n" + ControlOptions);
            Console.WriteLine("Starting Status:\n\tCard: " + Status.Card + "\n\tError: " + Status.ErrorCode);
            //
            ACR122UManager.GlobalCardCheck = (e) =>
            {
                bool CeckSuccess = false;
                if (e.ATR.Length == AcceptedATR.Length)
                {
                    CeckSuccess = true;
                    for (int i = 0; i < e.ATR.Length; i++)
                    {
                        if (e.ATR[i] != AcceptedATR[i])
                        {
                            CeckSuccess = false;
                            break;
                        }
                    }
                }
                return CeckSuccess;
            };

            Manager.CheckCard = true;

            ManagerTest Test = new ManagerTest(Manager);

            Manager.AcceptedCardScaned += Test.TestAccept;
            Manager.CardStateChanged += Test.TestStateChange;
            Manager.RejectedCardScaned += Test.TestRejected;
            Manager.CardDetected += Test.TestCardDetected;
            Manager.CardRemoved += Test.TestCardRemoved;
            List<string> Names = WinSmartCardContext.ListReadersAsStringsStatic();
            Console.ReadKey();

        }

        static class FileLogger
        {
            static readonly string Location = Environment.CurrentDirectory + "\\CardReaderOutput.txt";

            public static void WriteLine(string Write)
            {
                using (StreamWriter SW = new StreamWriter(File.Open(Location, FileMode.Append)))
                {
                    SW.WriteLine(Write);
                }
            }

            public static void WriteLine(string Write, params object[] obj)
            {
                using (StreamWriter SW = new StreamWriter(File.Open(Location, FileMode.Append)))
                {
                    SW.WriteLine(string.Format(Write, obj));
                }
            }

        }

        public class ManagerTest
        {

            ACR122UManager Manager;

            public ManagerTest(ACR122UManager M)
            {
                Manager = M;
            }

            public void TestStateChange(object sender, ACRCardStateChangeEventArg e)
            {
                Console.WriteLine("CardReaders state has changed");
                Console.WriteLine("State Enum : {0}", e.EventState);
                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
                Console.WriteLine("ATR : {0}", e.ATRString);
            }

            public void TestAccept(object sender, ACRCardAcceptedCardScanEventArg e)
            {
                Console.WriteLine("CardReader has accepted Card");
                Console.WriteLine("State Enum : {0}", e.EventState);
                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
                Console.WriteLine("ATR : {0}", e.ATRString);


                if (Manager.Card == null)
                {
                    ACR122U_NTAG215_SmartCard Card = Manager.ConnectToNTAGCard();
                    byte[] receivedUID = null;
                    Console.WriteLine(Card.GetcardUIDBytes(out receivedUID));
                    Console.WriteLine(BitConverter.ToString(receivedUID));
                    Manager.DisconnectToCard();
                }
                

            }

            public void TestRejected(object sender, ACRCardRejectedCardScanEventArg e)
            {
                Console.WriteLine("CardReader has rejected Card");
                Console.WriteLine("State Enum : {0}", e.EventState);
                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
                Console.WriteLine("ATR : {0}", e.ATRString);
            }

            public void TestCardDetected(object sender, ACRCardDetectedEventArg e)
            {
                Console.WriteLine("CardReader has detected Card");
                Console.WriteLine("State Enum : {0}", e.EventState);
                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
                Console.WriteLine("ATR : {0}", e.ATRString);
            }

            public void TestCardRemoved(object sender, ACRCardRemovedEventArg e)
            {
                Console.WriteLine("CardReader has removed Card");
                Console.WriteLine("State Enum : {0}", e.EventState);
                Console.WriteLine("State as Hex : {0:x}", (int)e.EventState);
                Console.WriteLine("ATR : {0}", e.ATRString);

                Manager.DisconnectToCard();
            }
        }

    }
}
