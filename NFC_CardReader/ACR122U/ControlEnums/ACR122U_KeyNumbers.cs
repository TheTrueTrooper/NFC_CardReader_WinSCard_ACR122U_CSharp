using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// A list of possible memory loctions to use with reader for keys
    /// </summary>
    public enum ACR122U_KeyMemories
    {
        /// <summary>
        /// Memory for your first key. Note these memory locations could  match to Key A or B... up to you
        /// </summary>
        Key1,
        /// <summary>
        /// Memory for your second key. Note these memory locations could  match to Key A or B... up to you
        /// </summary>
        Key2,
    }
}
