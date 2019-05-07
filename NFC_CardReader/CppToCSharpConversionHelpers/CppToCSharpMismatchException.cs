using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.CppToCSharpConversionHelpers
{
    class CppToCSharpMismatchException : Exception
    {
        public string MissmatchInfo { get; private set; }

        internal CppToCSharpMismatchException(string MissmatchInfoIn, string message, Exception innerException) : base(message, innerException)
        {
            MissmatchInfo = MissmatchInfoIn;
        }

        internal CppToCSharpMismatchException(string MissmatchInfoIn, string message) : base(message)
        {
            MissmatchInfo = MissmatchInfoIn;
        }

        internal CppToCSharpMismatchException(string MissmatchInfoIn) : base("C++ to CSharp missmatch detected: " + MissmatchInfoIn)
        {
            MissmatchInfo = MissmatchInfoIn;
        }
    }
}
