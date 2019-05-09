using NFC_CardReader.CppToCSharpConversionHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    public class WinSmartCardContext //: IDisposable
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

        bool Disposed = false;

        /// <summary>
        /// Creates a connect at the use scope requested
        /// </summary>
        /// <param name="Scope">the scope to use (often System)</param>
        public WinSmartCardContext(OperationScopes Scope, string ConnectedReaderName)
        {
            LastResultCode = WinSCard.SCardEstablishContext(Scope, ref _Context);
            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode);
            this.ConnectedReaderName = ConnectedReaderName;
        }

        /// <summary>
        /// Transmits a buffer as a command for the ADPU command formate mostly used by the ACR122
        /// </summary>
        /// <param name="SendCommand"></param>
        /// <param name="ReceivedResponse"></param>
        /// <param name="Protocol"></param>
        /// <returns></returns>
        public ErrorCodes Control(byte[] SendCommand, out byte[] ReceivedResponse, out bool HasCard, OperationScopes Scope = OperationScopes.SCARD_SCOPE_SYSTEM, SmartCardProtocols Protocol = SmartCardProtocols.SCARD_PROTOCOL_UNDEFINED)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            int TempCard = 0;
            int AProtocol = 0;
            uint IOTL = (uint)IOTLOperations.IOCTL_SMARTCARD_DIRECT; // 3225264;
            ReceivedResponse = new byte[256];

            int outBytes = ReceivedResponse.Length;

            if (Card == null)
            {
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Context Establish");
                LastResultCode = WinSCard.SCardConnect(_Context, ConnectedReaderName, SmartCardShareTypes.SCARD_SHARE_DIRECT, 0, ref TempCard, ref AProtocol);
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Connect");
                LastResultCode = WinSCard.SCardControl(TempCard, IOTL, SendCommand, ref ReceivedResponse, ref outBytes);
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Control");
                LastResultCode = WinSCard.SCardDisconnect(TempCard, SmartCardDispostion.SCARD_RESET_CARD);
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Card Release");
                Array.Resize(ref ReceivedResponse, outBytes);
                HasCard = false;
            }
            else
            {
                Card.Control(SendCommand, out ReceivedResponse, Scope, Protocol);
                HasCard = true;
            }
            //‭‭3136B0‬
            return LastResultCode;
        }
        
        /// <summary>
        /// Gets all the reader names without a context
        ///     Static Vers
        /// </summary>
        /// <returns>reader names</returns>
        public static List<string> ListReadersAsStringsStatic(bool ThrowOnNoReader = true)
        {
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();


            //Get count but due to the difference in language get only the count
            ErrorCodes LastResultCode = WinSCard.SCardListReaders(0, null, null, ref ReaderCount);

            if (!ThrowOnNoReader && LastResultCode == ErrorCodes.SCARD_E_NO_READERS_AVAILABLE)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring List Length retrieval");
            else if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
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
        /// Gets all the reader names with the context of current
        ///     Context vers
        /// </summary>
        /// <returns>reader names</returns>
        public List<string> ListReadersAsStrings(bool ThrowOnNoReader = true)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();


            //Get count but due to the difference in language get only the count
            LastResultCode = WinSCard.SCardListReaders(_Context, null, null, ref ReaderCount);


            if(!ThrowOnNoReader && LastResultCode == ErrorCodes.SCARD_E_NO_READERS_AVAILABLE)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring List Length retrieval");
            else if(LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
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
        /// Locks thread in a loop until time out or the state of card changes
        /// </summary>
        /// <param name="TimeOut">The Allow able time before time out</param>
        /// <param name="States">A array of states to watch (start as new() state setting readers name to the reader to watch)</param>
        /// <returns></returns>
        public ErrorCodes GetStatusChange(int TimeOut, ref ReadersCurrentState[] States)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            LastResultCode = WinSCard.SCardGetStatusChange(_Context, TimeOut, ref States, States.Count());
            return LastResultCode;
        }


        /// <summary>
        /// Connects to the reader with a card
        ///     Note Winscard requires a card
        /// </summary>
        /// <param name="ReaderName">The name of the reader to connect to</param>
        /// <param name="SmartCardShareTypes">The perfered protocols to use</param>
        /// <returns></returns>
        public WinSmartCard CardConnect(SmartCardShareTypes SmartCardShareTypes, SmartCardShareTypes ShareType = SmartCardShareTypes.SCARD_SHARE_SHARED, SmartCardProtocols Protocols = SmartCardProtocols.SCARD_PROTOCOL_Any )
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            int Card = 0;
            int Protocol = 0;

            LastResultCode = WinSCard.SCardConnect(_Context, ConnectedReaderName, ShareType,
                      Protocols, ref Card, ref Protocol);

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
        }

        /// <summary>
        /// Disposal for using statements
        /// </summary>
        public void Dispose()
        {
            if (Card != null)
            {
                Card.Dispose();
            }
            if (!Disposed)
            {
                LastResultCode = WinSCard.SCardReleaseContext(_Context);
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nThrown clean up.");
                Disposed = true;
            }
        }

        /// <summary>
        /// garbage collection clean up
        /// </summary>
        ~WinSmartCardContext()
        {
            if (Card != null)
            {
                Card.Dispose();
            }
            if (!Disposed)
            {
                LastResultCode = WinSCard.SCardReleaseContext(_Context);
                if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                    throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nThrown clean up.");
                Disposed = true;
            }
        }

        /// <summary>
        /// a wrapper import of the (mostly)provided error to sting func/meothd
        /// </summary>
        /// <param name="ReturnCode"></param>
        /// <returns></returns>
        public static string GetSmartCardErrMsg(ErrorCodes ReturnCode)
        {
            return WinSCard.GetScardErrMsg(ReturnCode);
        }
    }
}
