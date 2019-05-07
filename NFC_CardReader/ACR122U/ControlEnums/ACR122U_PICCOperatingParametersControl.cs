using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public enum ACR122U_PICCOperatingParametersControl
    {
        AutoPICCPolling = 0x80,
        AutoATSGeneration = 0x40,
        PollingInterval = 0x20,
        Felica424K = 0x10,
        Felica212K = 0x08,
        Topaz = 0x04,
        ISO14443TypeB = 0x02,
        ISO14443TypeA = 0x01,
        AllOff = 0x00,
        AllOn = 0xFF
    }
}
