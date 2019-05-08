using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// A list of possible keys to use
    /// </summary>
    public enum ACR122U_Keys
    {
        /// <summary>
        /// Tradtionaly Key 'A' The read only key
        /// </summary>
        KeyA,
        /// <summary>
        /// Tradtionaly Key 'A' The read/write Master key key
        /// </summary>
        KeyB,
        /// <summary>
        /// Tradtionaly Key 'A' The read only key
        /// </summary>
        ReadKey = KeyA,
        /// <summary>
        /// Tradtionaly Key 'A' The read/write Master key key
        /// </summary>
        WriteReadKey = KeyB,
    }
}
