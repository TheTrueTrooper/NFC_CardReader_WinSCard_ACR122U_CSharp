using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public class ACR122U_SmartCardException : WinSCardException
    {
        public ACR122U_ResposeErrorCodes ACRErrorOnException { get; private set; }

        internal ACR122U_SmartCardException(ACR122U_ResposeErrorCodes ACRErrorOnExceptionIn, ErrorCodes ErrorOnExceptionIn, string message, Exception innerException) : base(ErrorOnExceptionIn, message, innerException)
        {
            ACRErrorOnException = ACRErrorOnExceptionIn;
        }

        internal ACR122U_SmartCardException(ACR122U_ResposeErrorCodes ACRErrorOnExceptionIn, ErrorCodes ErrorOnExceptionIn, string message) : base(ErrorOnExceptionIn, message)
        {
            ACRErrorOnException = ACRErrorOnExceptionIn;
        }

        internal ACR122U_SmartCardException(ACR122U_ResposeErrorCodes ACRErrorOnExceptionIn,  ErrorCodes ErrorOnExceptionIn) : base(ErrorOnExceptionIn, ACR122U_SmartCard.GetACRErrMsg(ACRErrorOnExceptionIn) + "\n\t" + WinSCard.GetScardErrMsg(ErrorOnExceptionIn))
        {
            ACRErrorOnException = ACRErrorOnExceptionIn;
        }
    }
}
