using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible protocols that Readers could use
    /// </summary>
    public enum SmartCardProtocols
    {
        /// <summary>
        /// There is no active protocol.
        /// </summary>
        SCARD_PROTOCOL_UNDEFINED = 0x00,          
        /// <summary>
        /// T=0 is the active protocol.
        /// </summary>
        SCARD_PROTOCOL_T0 = 0x01,                
        /// <summary>
        /// T=1 is the active protocol.
        /// </summary>
        SCARD_PROTOCOL_T1 = 0x02,                
        /// <summary>
        /// Raw is the active protocol.
        /// </summary>
        SCARD_PROTOCOL_RAW = 0x10000,
        /// <summary>
        /// T=15 is the active protocol.
        /// </summary>
        T15 = 0x0008,
        //SCARD_PROTOCOL_DEFAULT = 0x80000000      // Use implicit PTS and too big for enum
        /// <summary>
        /// use any protocol
        /// </summary>
        SCARD_PROTOCOL_Any = SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1 | SCARD_PROTOCOL_RAW
    }
}
