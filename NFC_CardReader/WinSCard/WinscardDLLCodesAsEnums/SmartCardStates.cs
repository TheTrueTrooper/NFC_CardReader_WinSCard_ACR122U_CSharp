using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible states that The SmartCard could have
    /// </summary>
    [Flags]
    public enum SmartCardStates
    {
        /// <summary>
        /// The application is unaware of the current state, and would like 
        /// to know. The use of this value results in an immediate return
        /// from state transition monitoring services. This is represented
        /// by all bits set to zero.
        /// </summary>
        SCARD_STATE_UNAWARE = 0x00,
        /// <summary>
        /// The application requested that this reader be ignored. No other
        /// bits will be set.
        /// </summary>
        SCARD_STATE_IGNORE = 0x01,
        /// <summary>
        /// This implies that there is a difference between the state 
        /// believed by the application, and the state known by the Service
        /// Manager.When this bit is set, the application may assume a
        /// significant state change has occurred on this reader.
        /// </summary>
        SCARD_STATE_CHANGED = 0x02,
        /// <summary>
        /// This implies that the given reader name is not recognized by
        /// the Service Manager. If this bit is set, then SCARD_STATE_CHANGED
        /// and SCARD_STATE_IGNORE will also be set.
        /// </summary>
        SCARD_STATE_UNKNOWN = 0x04,
        /// <summary>
        /// This implies that the actual state of this reader is not
        /// available. If this bit is set, then all the following bits are
        /// clear.
        /// </summary>
        SCARD_STATE_UNAVAILABLE = 0x08,
        /// <summary>
        ///  This implies that there is not card in the reader.  If this bit
        ///  is set, all the following bits will be clear.
        /// </summary>
        SCARD_STATE_EMPTY = 0x10,
        /// <summary>
        ///  This implies that there is a card in the reader.
        /// </summary>
        SCARD_STATE_PRESENT = 0x20,
        /// <summary>
        ///  This implies that there is a card in the reader with an ATR
        ///  matching one of the target cards. If this bit is set,
        ///  SCARD_STATE_PRESENT will also be set.  This bit is only returned
        ///  on the SCardLocateCard() service.
        /// </summary>
        SCARD_STATE_ATRMATCH = 0x40,
        /// <summary>
        ///  This implies that the card in the reader is allocated for 
        ///  exclusive use by another application. If this bit is set,
        ///  SCARD_STATE_PRESENT will also be set.
        /// </summary>
        SCARD_STATE_EXCLUSIVE = 0x80,
        /// <summary>
        ///  This implies that the card in the reader is in use by one or 
        ///  more other applications, but may be connected to in shared mode. 
        ///  If this bit is set, SCARD_STATE_PRESENT will also be set.
        /// </summary>
        SCARD_STATE_INUSE = 0x100,
        /// <summary>
        ///  This implies that the card in the reader is unresponsive or not
        ///  supported by the reader or software.
        /// </summary>
        SCARD_STATE_MUTE = 0x200,
        /// <summary>
        ///  This implies that the card in the reader has not been powered up.
        /// </summary>
        SCARD_STATE_UNPOWERED = 0x400,
    }
}
