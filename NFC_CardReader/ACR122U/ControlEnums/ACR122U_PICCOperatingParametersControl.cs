using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// Flages to mark the the PICC Parameters such as cards to listen for and polling options.
    /// </summary>
    [Flags]
    public enum ACR122U_PICCOperatingParametersControl
    {
        /// <summary>
        /// Should the card reader auto poll or should it wait for commands
        /// </summary>
        AutoPICCPolling = 0x80,
        /// <summary>
        /// Should the system auto generate ATS
        /// </summary>
        AutoATSGeneration = 0x40,
        /// <summary>
        /// A flag to mark the the time before polling
        ///     on  = 255
        ///     off = 500
        /// </summary>
        PollingInterval = 0x20,
        /// <summary>
        /// Should the card listen for Falica424 cards
        ///     on  = Listen for
        ///     off = Ignore
        /// </summary>
        Felica424K = 0x10,
        /// <summary>
        /// Should the card listen for Falica212 cards
        ///     on  = Listen for
        ///     off = Ignore
        /// </summary>
        Felica212K = 0x08,
        /// <summary>
        /// Should the card listen for Topaz cards
        ///     on  = Listen for
        ///     off = Ignore
        /// </summary>
        Topaz = 0x04,
        /// <summary>
        /// Should the card listen for ISO1443B or Mifare Cards
        ///     on  = Listen for
        ///     off = Ignore
        /// </summary>
        ISO14443TypeB = 0x02,
        /// <summary>
        /// Should the card listen for ISO1443A or Mifare Cards
        ///     on  = Listen for
        ///     off = Ignore
        /// </summary>
        ISO14443TypeA = 0x01,
        /// <summary>
        /// Simply mark all as on
        /// </summary>
        AllOff = 0x00,
        /// <summary>
        /// Simply mark all as off
        /// </summary>
        AllOn = 0xFF
    }
}
