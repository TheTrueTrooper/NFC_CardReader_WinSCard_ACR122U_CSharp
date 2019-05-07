using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// The Modulation Type 
    /// </summary>
    public enum ACR122U_StatusModulationType
    {
        /// <summary>
        /// The Modulation Type is ISO1443 or Mifare
        /// </summary>
        ISO1443orMifare = 0x00,
        /// <summary>
        /// The Modulation Type is Active Mode
        /// </summary>
        ActiveMode = 0x01,
        /// <summary>
        /// The Modulation Type is Innovision Jewel Tag
        /// </summary>
        InnovisionJewelTag = 0x02,
        /// <summary>
        /// Extra Value for no card detected. is bigger than a byte so no risk of colish
        /// </summary>
        NoCardDetected= 0xFFF
    }
}
