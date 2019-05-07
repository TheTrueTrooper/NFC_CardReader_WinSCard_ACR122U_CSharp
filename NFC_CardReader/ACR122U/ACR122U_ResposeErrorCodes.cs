using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public enum ACR122U_ResposeErrorCodes
    {
        /// <summary>
        /// A Basic success. Note some fuctions have the trailing SW byte return the settings after. Or some have extra strange data in it.
        /// </summary>
        Success = 0x9000,
        /// <summary>
        /// Basic Error
        /// </summary>
        Error = 0x6300,
        /// <summary>
        /// The attempted function is not supported on this platform or firmware
        /// </summary>
        FuctionNotSupported = 0x6A81,
        /// <summary>
        /// Added Error to Mark an event were as the Winscard throws before Acr122u Reader; 
        ///     Extra full bite will ensure no collision.
        /// </summary>
        WinSCardError = 0xFFFFFF
    }
}
