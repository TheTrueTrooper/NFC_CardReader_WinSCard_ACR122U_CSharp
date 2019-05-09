using NFC_CardReader.CppToCSharpConversionHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// A class to Wrap all the wincard dll in very C# friendly wrapping
    /// </summary>
    public class WinSmartCard //: IDisposable
    {
        /// <summary>
        /// A Pointer to the card as an int since C# doesn't really directly reference it
        /// </summary>
        int _Card;

        /// <summary>
        /// Getting a Pointer to the card as an int for internal use; since C# doesn't really directly reference it
        /// </summary>
        internal int Card { get { return _Card; } }

        /// <summary>
        /// Is this card locked for a transaction
        /// </summary>
        public bool IsInTransaction { get; private set; } = false;

        /// <summary>
        /// Is the parent context and this card both alive
        /// </summary>
        public bool IsAliveWithContext { get; private set; } = true;

        /// <summary>
        /// A reference to the parent context
        /// </summary>
        public WinSmartCardContext Parent { get; private set; }

        /// <summary>
        /// the Protocol in use
        /// </summary>
        public SmartCardProtocols Protocol { get; private set; }

        /// <summary>
        /// The Last Call to winscards result
        /// </summary>
        public ErrorCodes LastResultCode { get; protected set; } = ErrorCodes.SCARD_S_SUCCESS;

        /// <summary>
        /// A Read only arry for this cards ATR
        /// </summary>
        public IList<byte> ATR { get; private set; }

        /// <summary>
        /// Gets ATR as a friendly string
        /// </summary>
        public string ATRString { get => BitConverter.ToString(ATR.ToArray()); }

        public bool Disposed { get; private set; } = false;

        /// <summary>
        /// Constructor for internal use
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Card"></param>
        internal WinSmartCard(WinSmartCardContext Parent, int Card)
        {
            this.Parent = Parent;
            _Card = Card;
            SmartCardProtocols ProtocolOut;
            SmartCardStatus SmartCardStatusOut;
            byte[] ReaderName;
            byte[] ATRRet;
            GetStatus(out ReaderName, out SmartCardStatusOut, out ProtocolOut, out ATRRet);
            ATR = Array.AsReadOnly(ATRRet);
            Protocol = ProtocolOut;
        }

        /// <summary>
        /// Gets the cards status with the ATR as a bytes 
        /// </summary>
        /// <param name="NameOfReader">NameOfReader</param>
        /// <param name="Status">Status of that reader</param>
        /// <param name="Protocol">Protocol in use</param>
        /// <param name="ATR">Its ATR as bytes</param>
        /// <returns>The Result of calls made to to the winscard.dll</returns>
        public ErrorCodes GetStatus(out byte[] NameOfReader, out SmartCardStatus Status, out SmartCardProtocols Protocol, out byte[] ATR)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            Status = SmartCardStatus.SCARD_UNKNOWN;
            Protocol = SmartCardProtocols.SCARD_PROTOCOL_UNDEFINED;
            int LengthOfName = 0;
            int LengthOfATR = 0;
            LastResultCode = WinSCard.SCardStatus(_Card, null, ref LengthOfName, ref Status, ref Protocol, null, ref LengthOfATR);

            ATR = new byte[LengthOfATR];
            NameOfReader = new byte[LengthOfName];

            LastResultCode = WinSCard.SCardStatus(_Card, NameOfReader, ref LengthOfName, ref Status, ref Protocol, ATR, ref LengthOfATR);

            return LastResultCode;
        }

        /// <summary>
        /// Gets the cards status with the ATR as a string 
        /// </summary>
        /// <param name="NameOfReader">NameOfReader</param>
        /// <param name="Status">Status of that reader</param>
        /// <param name="Protocol">Protocol in use</param>
        /// <param name="ATR">Its ATR as bytes</param>
        /// <returns>The Result of calls made to to the winscard.dll</returns>
        public ErrorCodes GetStatus(out string NameOfReader, out SmartCardStatus Status, out SmartCardProtocols Protocol, out string ATR)
        {
            byte[] ATRBytes;
            byte[] NameBytes;
            LastResultCode = GetStatus(out NameBytes, out Status, out Protocol, out ATRBytes);
            ATR = BitConverter.ToString(ATRBytes);
            NameOfReader = CppToCSharpHelpers.StringsFromNullTerminatedByteBuffer(NameBytes);

            return LastResultCode;
        }

        /// <summary>
        /// Gets the attribute support stopped as current reader doesn't support
        /// </summary>
        /// <param name="Attribute">he attribute to get</param>
        /// <param name="AttrOut">it's value as bytes</param>
        /// <returns></returns>
        public virtual ErrorCodes GetAttrib(SmartCardATR Attribute, out byte[] AttrOut)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            int AttrLength = 0;
            LastResultCode = WinSCard.SCardGetAttrib(_Card, Attribute, null, ref AttrLength);
            AttrOut = new byte[AttrLength];
            LastResultCode = WinSCard.SCardGetAttrib(_Card, Attribute, AttrOut, ref AttrLength);
            return LastResultCode;
        }

        /// <summary>
        /// Gets the attribute support stopped as current reader doesn't support
        /// </summary>
        /// <param name="Attribute">he attribute to get</param>
        /// <param name="AttrOut">it's value as a string</param>
        /// <returns></returns>
        public virtual ErrorCodes GetAttrib(SmartCardATR Attribute, out string AttrOut, bool IsBytes = false)
        {
            byte[] AttrBytes;
            LastResultCode = GetAttrib(Attribute, out AttrBytes);

            if (IsBytes)
                AttrOut = BitConverter.ToString(AttrBytes);
            else
                AttrOut = CppToCSharpHelpers.StringsFromNullTerminatedByteBuffer(AttrBytes);
            return LastResultCode;
        }

        /// <summary>
        /// Transmits a buffer as a command for the ADPU command formate mostly used by the ACR122
        /// </summary>
        /// <param name="SendCommand">the command to send as bytes</param>
        /// <param name="ReceivedResponse">the respose recived as bytes</param>
        /// <param name="Protocol">The protocol to use Default to null selecting the instances protocol</param>
        /// <returns></returns>
        public ErrorCodes TransmitData(byte[] SendCommand, out byte[] ReceivedResponse, SmartCardProtocols? Protocol = null)
        {
            if (Protocol == null)
                Protocol = this.Protocol;

            ReceivedResponse = new byte[256];
            SCARD_IO_REQUEST request = new SCARD_IO_REQUEST();
            request.dwProtocol = (int)Protocol.Value;
            request.cbPciLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(SCARD_IO_REQUEST));

            int outBytes = ReceivedResponse.Length;
            LastResultCode = WinSCard.SCardTransmit(Card, ref request, ref SendCommand[0], SendCommand.Length, ref request, ref ReceivedResponse[0], ref outBytes);

            if (LastResultCode != ErrorCodes.SCARD_S_SUCCESS)
                throw new WinSCardException(LastResultCode, WinSCard.GetScardErrMsg(LastResultCode) + "\nError perceived durring Actual ATR srting retrieval");

            Array.Resize(ref ReceivedResponse, outBytes);

            return LastResultCode;
        }

        /// <summary>
        /// Locks the card for a transaction with steps
        /// </summary>
        /// <returns></returns>
        public ErrorCodes BeginTransaction()
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            IsInTransaction = true;
            return WinSCard.SCardBeginTransaction(_Card);
        }

        /// <summary>
        /// Ends a transaction
        /// </summary>
        /// <param name="Dispostion"></param>
        /// <returns></returns>
        public ErrorCodes BeginTransaction(SmartCardDispostion Dispostion)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            IsInTransaction = false;
            return WinSCard.SCardEndTransaction(_Card, Dispostion);
        }


        /// <summary>
        /// Sends a Raw byte buffer for commands that fall out side of the ADPU or are sudo ADPU
        /// </summary>
        /// <param name="SendCommand">The Command to send as bytes</param>
        /// <param name="ReceivedResponse">The ReceivedResponse as bytes</param>
        /// <param name="Scope">The Scope to use defualted to System</param>
        /// <param name="Protocol">The protocol to use Default to null selecting the instances protocol</param>
        /// <returns></returns>
        public ErrorCodes Control(byte[] SendCommand, out byte[] ReceivedResponse, OperationScopes Scope = OperationScopes.SCARD_SCOPE_SYSTEM, SmartCardProtocols? Protocol = null)
        {
            if (Disposed)
                throw new ObjectDisposedException("WinSmartCardContext");
            if (Protocol == null)
                Protocol = this.Protocol;

            uint IOTL = (int)IOTLOperations.IOCTL_SMARTCARD_DIRECT; // 3225264;
            ReceivedResponse = new byte[256];


            int outBytes = ReceivedResponse.Length;
            LastResultCode = WinSCard.SCardControl(_Card, IOTL, SendCommand, ref ReceivedResponse, ref outBytes);

            Array.Resize(ref ReceivedResponse, outBytes);
            //‭‭3136B0‬
            return LastResultCode;
        }

        /// <summary>
        /// Destroys Card and notifies parent
        /// </summary>
        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                IsAliveWithContext = false;
                Parent.NotifyOfCardsDeath();
                WinSCard.SCardDisconnect(_Card, SmartCardDispostion.SCARD_RESET_CARD);
            }
        }

        /// <summary>
        /// Destroys Card in garbage and notifies parent
        /// </summary>
        ~WinSmartCard()
        {
            if (!Disposed)
            {
                Disposed = true;
                IsAliveWithContext = false;
                Parent.NotifyOfCardsDeath();
                WinSCard.SCardDisconnect(_Card, SmartCardDispostion.SCARD_RESET_CARD);
            }
        }
    }
}
