using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// Marks the T1 and T2 cycles for LED controlls
    /// </summary>
    public enum ACR122U_BuzzerControl
    {
        /// <summary>
        /// Keep buzzer off
        /// </summary>
        BuzzerOff = 0x00,
        /// <summary>
        /// Buzz during T1 cycle
        /// </summary>
        BuzzerOnT1Cycle = 0x02,
        /// <summary>
        /// Buzz during T2 cycle
        /// </summary>
        BuzzerOnT2Cycle = 0x01,
        /// <summary>
        /// Buzz during both T1 and T2 cycles
        /// </summary>
        BuzzerOnT1And2Cycle = 0x03
    }
}
