using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// A class for the raw import of SCARD_IO_REQUEST
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SCARD_IO_REQUEST
    {
        public int dwProtocol;
        public int cbPciLength;
    }

    /// <summary>
    /// A class for the raw import of APDURec
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct APDURec
    {
        public byte bCLA;
        public byte bINS;
        public byte bP1;
        public byte bP2;
        public byte bP3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] SW;
        public bool IsSend;
    }

    /// <summary>
    /// A class for the raw import of SCARD_READERSTATE
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SCARD_READERSTATE
    {
        public string szReader;
        public int pvUserData;
        public int dwCurrentState;
        public int dwEventState;
        public int cbAtr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] rgbAtr;
    }
    //UnmanagedType.ByValArray, SizeConst:=36

    /// <summary>
    /// A class that imports the Winscard.dll mostly raw with small tweaks to make it more friendly but still maint original form
    /// </summary>
    internal static class WinSCard
    {
        /// <summary>
        /// A actual constant that marks the length of a ATR Note windows is actually 36 UNIX/Linux ia 33
        /// </summary>
        internal const int SCARD_ATR_LENGTH = 36;//33;

        // All of the imported DLL files
        [DllImport("winscard.dll", EntryPoint = "SCardEstablishContext")]
        static extern int SCardEstablishContextImport(int dwScope, int pvReserved1, int pvReserved2, ref int phContext);
        [DllImport("winscard.dll", EntryPoint = "SCardConnectA")]
        static extern int SCardConnectImport(int hContext, string szReaderName, int dwShareMode, int dwPrefProtocol, ref int phCard, ref int ActiveProtocol);
        [DllImport("winscard.dll", EntryPoint = "SCardReleaseContext")]
        static extern int SCardReleaseContextImport(int phContext);
        [DllImport("winscard.dll", EntryPoint = "SCardBeginTransaction")]
        static extern int SCardBeginTransactionImport(int hCard);
        [DllImport("winscard.dll", EntryPoint = "SCardDisconnect")]
        static extern int SCardDisconnect(int hCard, int Disposition);
        [DllImport("winscard.dll", EntryPoint = "SCardListReaderGroups")]
        static extern int SCardListReaderGroupsImport(int hContext, ref string mzGroups, ref int pcchGroups);
        [DllImport("winscard.DLL", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        static extern int SCardListReadersImport(int hContext, byte[] Groups, byte[] Readers, ref int pcchReaders);
                                             //(SCARDCONTEXT hContext, LPSTR mszGroups,LPDWORD pcchGroups);
        [DllImport("winscard.dll", EntryPoint = "SCardStatusA")]
        static extern int SCardStatusImport(int hCard, byte[] mszReaderNames, ref int pcchReaderLen, ref int pdwState, ref int pdwProtocol, byte[] pbAtr, ref int pcbAtrLen);
                                      //(SCARDHANDLE hCard, LPSTR mszReaderNames, LPDWORD pcchReaderLen, LPDWORD pdwState, LPDWORD pdwProtocol, LPBYTE pbAtr, LPDWORD pcbAtrLen);
        [DllImport("winscard.dll", EntryPoint = "SCardEndTransaction")]
        static extern int SCardEndTransactionImport(int hCard, int Disposition);
        [DllImport("WinScard.dll", EntryPoint = "SCardTransmit")]
        static extern int SCardTransmitImport(int hCard, ref SCARD_IO_REQUEST pioSendPci, ref byte pbSendBuffer, int cbSendLength, ref SCARD_IO_REQUEST pioRecvPci, ref byte pbRecvBuffer, ref int pcbRecvLength);
        [DllImport("winscard.dll", EntryPoint = "SCardControl")]
        static extern int SCardControlImport(int hCard, uint dwControlCode, ref byte SendBuff, int SendBuffLen, ref byte RecvBuff, int RecvBuffLen, ref int pcbBytesReturned);
        [DllImport("winscard.dll", EntryPoint = "SCardGetStatusChangeA", CharSet = CharSet.Ansi)]
        static extern int SCardGetStatusChangeImport(int hContext, int dwTime, ref SCARD_READERSTATE rgReaderState, int cReaders);
        //                                  (int hContext, int dwTime, SCARDREADER_STATE rgReaderState, int cReaders)
        [DllImport("winscard.dll", EntryPoint = "SCardGetAttrib")]
        static extern int SCardGetAttribImport(int hCard, int dwAttrId, byte[] pbAttr, ref int pcbAttrLen);
                                    //(SCARDHANDLE hCard, DWORD dwAttrId, LPBYTE pbAttr, LPDWORD pcbAttrLen);
         
        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Scope">Scope</param>
        /// <param name="Context">Context</param>
        /// <returns>The Error Code</returns>
        internal static ErrorCodes SCardEstablishContext(OperationScopes Scope, ref int Context)
        {
            return (ErrorCodes)SCardEstablishContextImport((int)Scope, 0, 0, ref Context);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Context">Context</param>
        /// <returns>The Error Code</returns>
        internal static ErrorCodes SCardReleaseContext(int Context)
        {
            return (ErrorCodes)SCardReleaseContextImport(Context);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="ReaderName">The Readers Name</param>
        /// <param name="ShareMode">The Perferd Sandbox</param>
        /// <param name="Protocol">The Perferd protocol</param>
        /// <param name="Card">The Card</param>
        /// <param name="ActiveProtocol">The Active protocol</param>
        /// <returns>The Error Code</returns>
        internal static ErrorCodes SCardConnect(int Context, string ReaderName, SmartCardShareTypes ShareMode, SmartCardProtocols Protocol, ref int Card, ref int ActiveProtocol)
        {
            return (ErrorCodes)SCardConnectImport(Context, ReaderName, (int)ShareMode, (int)Protocol, ref Card, ref ActiveProtocol);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContextC# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <returns></returns>
        internal static ErrorCodes SCardBeginTransaction(int Card)
        {
            return (ErrorCodes)SCardBeginTransactionImport(Card);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="Disposition"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardDisconnect(int Card, SmartCardDispostion Disposition)
        {
            return (ErrorCodes)SCardDisconnect(Card, (int)Disposition);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="Groups"></param>
        /// <param name="pcchGroups"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardListReaderGroups(int Context, ref string Groups, ref int pcchGroups)
        {
            return (ErrorCodes)SCardListReaderGroupsImport(Context, ref Groups, ref pcchGroups);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="Groups"></param>
        /// <param name="Readers"></param>
        /// <param name="pcchReaders"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardListReaders(int Context, byte[] Groups, byte[] Readers, ref int ReaderCountOut)
        {
            return (ErrorCodes)SCardListReadersImport(Context, Groups, Readers, ref ReaderCountOut);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="ReaderName"></param>
        /// <param name="pcchReaderLen"></param>
        /// <param name="State"></param>
        /// <param name="Protocol">The Perferd protocol</param>
        /// <param name="ATR"></param>
        /// <param name="ATRLen"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardStatus(int Card, byte[] ReaderName, ref int ReaderLength, ref SmartCardStatus Status, ref SmartCardProtocols Protocol, byte[] ATR, ref int ATRLen)
        {
            int StatusReturn = 0;
            int ProtocolReturn = 0;
            ErrorCodes ErrorCode = (ErrorCodes)SCardStatusImport(Card, ReaderName, ref ReaderLength, ref StatusReturn, ref ProtocolReturn, ATR, ref ATRLen);
            Status = (SmartCardStatus)StatusReturn;
            Protocol = (SmartCardProtocols)ProtocolReturn;
            return ErrorCode;
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="Disposition"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardEndTransaction(int Card, SmartCardDispostion Disposition)
        {
            return (ErrorCodes)SCardEndTransactionImport(Card, (int)Disposition);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="pioSendPci"></param>
        /// <param name="pbSendBuffer"></param>
        /// <param name="cbSendLength"></param>
        /// <param name="pioRecvPci"></param>
        /// <param name="pbRecvBuffer"></param>
        /// <param name="pcbRecvLength"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardTransmit(int Card, ref SCARD_IO_REQUEST pioSendPci, ref byte pbSendBuffer, int cbSendLength, ref SCARD_IO_REQUEST pioRecvPci, ref byte pbRecvBuffer, ref int pcbRecvLength)
        {
            return (ErrorCodes)SCardTransmitImport(Card, ref pioSendPci, ref pbSendBuffer, cbSendLength, ref pioRecvPci, ref pbRecvBuffer, ref pcbRecvLength);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="dwControlCode"></param>
        /// <param name="SendBuff"></param>
        /// <param name="SendBuffLen"></param>
        /// <param name="RecvBuff"></param>
        /// <param name="RecvBuffLen"></param>
        /// <param name="pcbBytesReturned"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardControl(int Card, uint dwControlCode, byte[] SendBuff, ref byte[] RecvBuff, ref int pcbBytesReturned)
        {
            return (ErrorCodes)SCardControlImport(Card, dwControlCode, ref SendBuff[0], SendBuff.Length, ref RecvBuff[0], RecvBuff.Length, ref pcbBytesReturned);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="TimeOut"></param>
        /// <param name="ReaderState"></param>
        /// <param name="ReaderCount"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardGetStatusChange(int Context, int TimeOut, ref ReadersCurrentState[] ReaderStates, int ReaderCount)
        {
            SCARD_READERSTATE[] UnsafeReaderStates = ReaderStates.ToSCARD_READERSTATE();
            ErrorCodes Return = (ErrorCodes)SCardGetStatusChangeImport(Context, TimeOut, ref UnsafeReaderStates[0], ReaderCount);
            ReaderStates = UnsafeReaderStates.ToReadersCurrentState();
            return Return;
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="Card">The Card</param>
        /// <param name="Attribute"></param>
        /// <param name="AttrOut"></param>
        /// <param name="AttrLen"></param>
        /// <returns></returns>
        internal static ErrorCodes SCardGetAttrib(int Card, SmartCardATR Attribute, byte[] AttrOut, ref int AttrLen)
        { 
            return (ErrorCodes)SCardGetAttribImport(Card, (int)Attribute, AttrOut, ref AttrLen);
        }

        /// <summary>
        /// C# Friendly quick wrapper Addtional wrapping can found at WinSmartCard and Win SmardCardContext
        /// </summary>
        /// <param name="ReturnCode"></param>
        /// <returns></returns>
        internal static string GetScardErrMsg(ErrorCodes ReturnCode)
        {
            switch (ReturnCode)
            {
                case ErrorCodes.SCARD_E_CANCELLED:
                    return ("The action was canceled by an SCardCancel request.");
                case ErrorCodes.SCARD_E_CANT_DISPOSE:
                    return ("The system could not dispose of the media in the requested manner.");
                case ErrorCodes.SCARD_E_CARD_UNSUPPORTED:
                    return ("The smart card does not meet minimal requirements for support.");
                case ErrorCodes.SCARD_E_DUPLICATE_READER:
                    return ("The reader driver didn't produce a unique reader name.");
                case ErrorCodes.SCARD_E_INSUFFICIENT_BUFFER:
                    return ("The data buffer for returned data is too small for the returned data.");
                case ErrorCodes.SCARD_E_INVALID_ATR:
                    return ("An ATR string obtained from the registry is not a valid ATR string.");
                case ErrorCodes.SCARD_E_INVALID_HANDLE:
                    return ("The supplied handle was invalid.");
                case ErrorCodes.SCARD_E_INVALID_PARAMETER:
                    return ("One or more of the supplied parameters could not be properly interpreted.");
                case ErrorCodes.SCARD_E_INVALID_TARGET:
                    return ("Registry startup information is missing or invalid.");
                case ErrorCodes.SCARD_E_INVALID_VALUE:
                    return ("One or more of the supplied parameter values could not be properly interpreted.");
                case ErrorCodes.SCARD_E_NOT_READY:
                    return ("The reader or card is not ready to accept commands.");
                case ErrorCodes.SCARD_E_NOT_TRANSACTED:
                    return ("An attempt was made to end a non-existent transaction.");
                case ErrorCodes.SCARD_E_NO_MEMORY:
                    return ("Not enough memory available to complete this command.");
                case ErrorCodes.SCARD_E_NO_SERVICE:
                    return ("The smart card resource manager is not running.");
                case ErrorCodes.SCARD_E_NO_SMARTCARD:
                    return ("The operation requires a smart card, but no smart card is currently in the device.");
                case ErrorCodes.SCARD_E_PCI_TOO_SMALL:
                    return ("The PCI receive buffer was too small.");
                case ErrorCodes.SCARD_E_PROTO_MISMATCH:
                    return ("The requested protocols are incompatible with the protocol currently in use with the card.");
                case ErrorCodes.SCARD_E_READER_UNAVAILABLE:
                    return ("The specified reader is not currently available for use.");
                case ErrorCodes.SCARD_E_READER_UNSUPPORTED:
                    return ("The reader driver does not meet minimal requirements for support.");
                case ErrorCodes.SCARD_E_SERVICE_STOPPED:
                    return ("The smart card resource manager has shut down.");
                case ErrorCodes.SCARD_E_SHARING_VIOLATION:
                    return ("The smart card cannot be accessed because of other outstanding connections.");
                case ErrorCodes.SCARD_E_SYSTEM_CANCELLED:
                    return ("The action was canceled by the system, presumably to log off or shut down.");
                case ErrorCodes.SCARD_E_TIMEOUT:
                    return ("The user-specified timeout value has expired.");
                case ErrorCodes.SCARD_E_UNKNOWN_CARD:
                    return ("The specified smart card name is not recognized.");
                case ErrorCodes.SCARD_E_UNKNOWN_READER:
                    return ("The specified reader name is not recognized.");
                case ErrorCodes.SCARD_F_COMM_ERROR:
                    return ("An internal communications error has been detected.");
                case ErrorCodes.SCARD_F_INTERNAL_ERROR:
                    return ("An internal consistency check failed.");
                case ErrorCodes.SCARD_F_UNKNOWN_ERROR:
                    return ("An internal error has been detected, but the source is unknown.");
                case ErrorCodes.SCARD_F_WAITED_TOO_LONG:
                    return ("An internal consistency timer has expired.");
                case ErrorCodes.SCARD_S_SUCCESS:
                    return ("No error was encountered.");
                case ErrorCodes.SCARD_W_REMOVED_CARD:
                    return ("The smart card has been removed, so that further communication is not possible.");
                case ErrorCodes.SCARD_W_RESET_CARD:
                    return ("The smart card has been reset, so any shared state information is invalid.");
                case ErrorCodes.SCARD_W_UNPOWERED_CARD:
                    return ("Power has been removed from the smart card, so that further communication is not possible.");
                case ErrorCodes.SCARD_W_UNRESPONSIVE_CARD:
                    return ("The smart card is not responding to a reset.");
                case ErrorCodes.SCARD_W_UNSUPPORTED_CARD:
                    return ("The reader cannot communicate with the card, due to ATR string configuration conflicts.");
                case ErrorCodes.SCARD_E_NO_READERS_AVAILABLE:
                    return ("There are no avaible smart cards.");
                default:
                    return ("?");
            }
        }

        
    }
}
