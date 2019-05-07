using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.CppToCSharpConversionHelpers
{
    internal static class CppToCSharpHelpers
    {

        /// <summary>
        /// Parses Null Terminated Strings from buffer;
        ///     That takes a ref of index to move with. Useful when parsing lists.
        /// </summary>
        /// <param name="Buffer">The byte buffer to parse</param>
        /// <returns>The null terminated string</returns>
        private static string StringsFromNullTerminatedByteBuffer(byte[] Buffer, ref int Index)
        {
            string String = "";
            if(Buffer.Count() > 0)
                while (Buffer[Index] != 0)
                {
                    String = String + (char)Buffer[Index];
                    Index = Index + 1;
                }
            return String;
        }

        /// <summary>
        /// Parses Null Terminated Strings from buffer
        /// </summary>
        /// <param name="Buffer">The byte buffer to parse</param>
        /// <returns>The null terminated string</returns>
        internal static string StringsFromNullTerminatedByteBuffer(byte[] Buffer)
        {
            int Index = 0;
            return StringsFromNullTerminatedByteBuffer(Buffer, ref Index);
        }

        /// <summary>
        /// A quick and dirty null terminated string and list parser
        /// </summary>
        /// <param name="Buffer">The byte buffer to parse</param>
        /// <returns>The List of null terminated strings</returns>
        internal static List<string> ListOfStringsFromNullTerminatedByteBuffer(byte[] Buffer)
        {
            List<string> CompiledList = new List<string>();

            string String = "";
            int Index = 0;
            // if we have a count find the count

            // Convert reader buffer to string using null string terminators
            while (Buffer[Index] != 0)
            {
                // if we have a count find the count
                String = StringsFromNullTerminatedByteBuffer(Buffer, ref Index);

                //Add reader name to list after perceived null string terminators
                CompiledList.Add(String);
                //rest and move forward to check for more
                String = "";
                Index = Index + 1;

            }

            return CompiledList;
        }

        /// <summary>
        /// A quick and dirty null terminated string and list parser;
        ///     That is also conscious of the count to check results.
        /// </summary>
        /// <param name="Buffer">The byte buffer to parse</param>
        /// <returns>The List of null terminated strings</returns>
        internal static List<string> ListOfStringsFromNullTerminatedByteBuffer(byte[] Buffer, int Count)
        {
            List<string> CompiledList = new List<string>();

            if (Count > 0)
                CompiledList = ListOfStringsFromNullTerminatedByteBuffer(Buffer);

            if (Count != CompiledList.Count)
                throw new CppToCSharpMismatchException("Warning perceived mismatch on a List Of Strings from buffer:\n\tCount Check:" + Count + "\n\tCompiled List Count " + CompiledList.Count, "Warning perceived mismatch on a ListOfStringsFromNullTerminatedByteBuffer() Call:\n\tCount in doesnt match count out");

            return CompiledList;
        }
    }
}
