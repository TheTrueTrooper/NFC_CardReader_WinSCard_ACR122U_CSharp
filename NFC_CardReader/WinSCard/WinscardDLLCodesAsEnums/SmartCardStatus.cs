using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible stats of the Readers Card
    /// </summary>
    public enum SmartCardStatus
    {
        /// <summary>
        /// This value implies the driver is unaware of the current 
        /// state of the reader.
        /// </summary>
        SCARD_UNKNOWN = 0,
        /// <summary>
        /// This value implies there is no card in the reader.
        /// </summary>
        SCARD_ABSENT = 1,
        /// <summary>
        /// This value implies there is a card is present in the reader, 
        /// but that it has not been moved into position for use.
        /// </summary>
        SCARD_PRESENT = 2,
        /// <summary>
        /// This value implies there is a card in the reader in position 
        /// for use.  The card is not powered.
        /// </summary>
        SCARD_SWALLOWED = 3,
        /// <summary>
        /// This value implies there is power is being provided to the card, 
        /// but the Reader Driver is unaware of the mode of the card.
        /// </summary>
        SCARD_POWERED = 4,
        /// <summary>
        /// This value implies the card has been reset and is awaiting 
        /// PTS negotiation.
        /// </summary>
        SCARD_NEGOTIABLE = 5,
        /// <summary>
        /// This value implies the card has been reset and specific 
        /// communication protocols have been established.
        /// </summary>
        SCARD_SPECIFIC = 6
    }
}
