using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible ways for an application to assume contol over a card 
    /// </summary>
    public enum SmartCardShareTypes
    {
        /// <summary>
        /// This application is not willing to share this card with other 
        /// applications.
        /// </summary>
        SCARD_SHARE_EXCLUSIVE = 1,
        /// <summary>
        /// This application is willing to share this card with other 
        /// applications.
        /// </summary>
        SCARD_SHARE_SHARED = 2,
        /// <summary>
        /// This application demands direct control of the reader, so it 
        /// is not available to other applications.
        /// </summary>
        SCARD_SHARE_DIRECT = 3,
    }
}
