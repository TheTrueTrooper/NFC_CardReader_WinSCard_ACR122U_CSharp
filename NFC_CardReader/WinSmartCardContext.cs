using NFC_CardReader.CppToCSharpConversionHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    public class WinSmartCardContext : IDisposable
    {
        /// <summary>
        /// A Pointer to the a context as an int since C# doesn't really directly reference it
        /// </summary>
        int _Context;

        /// <summary>
        /// Internal Getter for the Pointer to the a context as an int since C# doesn't really directly reference it
        /// </summary>
        internal int Context { get { return _Context; } }

        /// <summary>
        /// The card sitting on the Reader
        /// </summary>
        public WinSmartCard Card { get; protected set; } = null;

        /// <summary>
        /// The Reader we are usings name
        /// </summary>
        public string ConnectedReaderName { get; private set; }

        /// <summary>
        /// The Result of the last call to the winscard
        /// </summary>
        public ErrorCodes LastResultCode { get; protected set; } = ErrorCodes.SCARD_S_SUCCESS;

        public WinSmartCardContext(OperationScopes Scope)
        {
            LastResultCode = WinSCard.SCardEstablishContext(Scope, ref _Context);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode);
        }

        /// <summary>
        /// Gets all the reader names with the context of current
        ///     Context vers
        /// </summary>
        /// <returns>reader names</returns>
        public List<string> ListReadersAsStrings()
        {
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();


            //Get count but due to the difference in language get only the count
            LastResultCode = WinSCard.SCardListReaders(_Context, null, null, ref ReaderCount);

            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring List Length retrieval");

            //Alot space for the List
            byte[] ReadersList = new byte[ReaderCount];

            //Fill the list with the actual values
            LastResultCode = WinSCard.SCardListReaders(_Context, null, ReadersList, ref ReaderCount);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Actual List retrieval");

            return CppToCSharpHelpers.ListOfStringsFromNullTerminatedByteBuffer(ReadersList);

        }

        /// <summary>
        /// Gets all the reader names without a context
        ///     Static Vers
        /// </summary>
        /// <returns>reader names</returns>
        public static List<string> ListReadersAsStringsStatic()
        {
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();


            //Get count but due to the difference in language get only the count
            ErrorCodes LastResultCode = WinSCard.SCardListReaders(0, null, null, ref ReaderCount);

            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring List Count retrieval");

            //Alot space for the List
            byte[] ReadersList = new byte[ReaderCount];

            //Fill the list with the actual values
            LastResultCode = WinSCard.SCardListReaders(0, null, ReadersList, ref ReaderCount);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Actual List retrieval");

            return CppToCSharpHelpers.ListOfStringsFromNullTerminatedByteBuffer(ReadersList);

        }

        /// <summary>
        /// Locks thread in a loop until time out or the state of card changes
        /// </summary>
        /// <param name="TimeOut"></param>
        /// <param name="States"></param>
        /// <returns></returns>
        public ErrorCodes GetStatusChange(int TimeOut, ref ReadersCurrentState[] States)
        {
            LastResultCode = WinSCard.SCardGetStatusChange(_Context, TimeOut, ref States, States.Count());
            return LastResultCode;
        }


        /// <summary>
        /// Connects to the reader with a card
        ///     Note Winscard requires a card
        /// </summary>
        /// <param name="ReaderName"></param>
        /// <param name="SmartCardShareTypes"></param>
        /// <returns></returns>
        public WinSmartCard Connect(string ReaderName, SmartCardShareTypes SmartCardShareTypes)
        {

            ConnectedReaderName = ReaderName;
            int Card = 0;
            int Protocol = 0;

            LastResultCode = WinSCard.SCardConnect(_Context, ConnectedReaderName, SmartCardShareTypes.SCARD_SHARE_SHARED,
                      SmartCardProtocols.SCARD_PROTOCOL_T0 | SmartCardProtocols.SCARD_PROTOCOL_T1, ref Card, ref Protocol);

            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode);

            this.Card = new WinSmartCard(this, Card);

            return this.Card;
        }

        /// <summary>
        /// Notify for child card to notify parent of death
        /// </summary>
        internal void NotifyOfCardsDeath()
        {
            Card = null;
            ConnectedReaderName = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (Card != null)
                Card.Dispose();
            LastResultCode = WinSCard.SCardReleaseContext(_Context);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nThrown clean up.");
            if (Card != null)
                Card.Dispose();
        }

        ~WinSmartCardContext()
        {
            if (Card != null)
                Card.Dispose();
            LastResultCode = WinSCard.SCardReleaseContext(_Context);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nThrown clean up.");
        }


        public static string GetSmartCardErrMsg(ErrorCodes ReturnCode)
        {
            return WinSCard.GetScardErrMsg(ReturnCode);
        }
    }
}
