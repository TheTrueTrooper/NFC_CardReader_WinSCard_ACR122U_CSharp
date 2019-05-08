using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// An exception to throw in regards to errors from the wincard dll
    /// </summary>
    public class WinSCardException : Exception
    {
        /// <summary>
        /// The Error code on exception
        /// </summary>
        public ErrorCodes ErrorOnException { get; private set; }

        internal WinSCardException(ErrorCodes ErrorOnExceptionIn, string message, Exception innerException) : base(message, innerException)
        {
            ErrorOnException = ErrorOnExceptionIn;
        }

        internal WinSCardException(ErrorCodes ErrorOnExceptionIn, string message) : base(message)
        {
            ErrorOnException = ErrorOnExceptionIn;
        }

        internal WinSCardException(ErrorCodes ErrorOnExceptionIn) : base(WinSCard.GetScardErrMsg(ErrorOnExceptionIn))
        {
            ErrorOnException = ErrorOnExceptionIn;
        }
    }
}
