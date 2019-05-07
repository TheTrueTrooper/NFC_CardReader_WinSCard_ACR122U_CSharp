using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible ways Readers could let go of SmartCards
    /// </summary>
    public enum SmartCardDispostion
    {
        /// <summary>
        /// Don't do anything special on close
        /// </summary>
        SCARD_LEAVE_CARD = 0,
        /// <summary>
        /// Reset the card on close
        /// </summary>
        SCARD_RESET_CARD = 1,
        /// <summary>
        /// Power down the card on close
        /// </summary>
        SCARD_UNPOWER_CARD = 2,
        /// <summary>
        /// Eject the card on close
        /// </summary>
        SCARD_EJECT_CARD = 3, 
    }
}
