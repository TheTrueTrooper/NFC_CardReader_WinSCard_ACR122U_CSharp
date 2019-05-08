using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// IOTL Operations not going to go through tem as I have yet to need them?
    /// </summary>
    public enum IOTLOperations
    {
        IOCTL_SMARTCARD_DIRECT = 3225264,//0x312008,
        IOCTL_SMARTCARD_SELECT_SLOT = 0x31200C,
        IOCTL_SMARTCARD_DRAW_LCDBMP = 0x312010,
        IOCTL_SMARTCARD_DISPLAY_LCD = 0x312014,
        IOCTL_SMARTCARD_CLR_LCD = 0x312018,
        IOCTL_SMARTCARD_READ_KEYPAD = 0x31201C,
        IOCTL_SMARTCARD_READ_RTC = 0x312024,
        IOCTL_SMARTCARD_SET_RTC = 0x312028,
        IOCTL_SMARTCARD_SET_OPTION = 0x31202C,
        IOCTL_SMARTCARD_SET_LED = 0x312030,
        IOCTL_SMARTCARD_LOAD_KEY = 0x312038,
        IOCTL_SMARTCARD_READ_EEPROM = 0x312044,
        IOCTL_SMARTCARD_WRITE_EEPROM = 0x312048,
        IOCTL_SMARTCARD_GET_VERSION = 0x318268,
        IOCTL_SMARTCARD_GET_READER_INFO = 0x31200C,
        IOCTL_SMARTCARD_SET_CARD_TYPE = 0x312030,
        IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND = 0x318316,
    }
}
