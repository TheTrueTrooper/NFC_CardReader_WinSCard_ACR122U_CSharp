using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    internal class StaticSharedErrors
    {
        internal static string IdexingError = "Your index was out of bounds. In a Block 16 there are only 16 memory locations.\nPlease select a number between 0 and 15, as per the zero-indexing.";
        internal static string AthenticationWriteError = "The block you are trying to write has yet to be athenticated with a B Key.";
        internal static string AthenticationReadError = "The block you are trying to read has yet to be athenticated with a A or B Key.";
    }
}
