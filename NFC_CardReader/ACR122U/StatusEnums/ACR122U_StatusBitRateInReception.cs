using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// The Bit Rate in reception
    /// </summary>
    public enum ACR122U_StatusBitRateInReception
    {
        /// <summary>
        /// The Bit Rate in reception is 106kbps
        /// </summary>
        Is106kbps = 0x00,
        /// <summary>
        /// The Bit Rate in reception is 212kbps
        /// </summary>
        Is212kbps = 0x01,
        /// <summary>
        /// The Bit Rate in reception is 424kbps
        /// </summary>
        Is424kbps = 0x02,
        /// <summary>
        /// Extra Value for no card detected. is bigger than a byte so no risk of colish
        /// </summary>
        NoReception = 0xFFF
    }
}
