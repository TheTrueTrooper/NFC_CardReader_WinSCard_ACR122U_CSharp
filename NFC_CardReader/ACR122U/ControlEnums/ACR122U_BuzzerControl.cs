using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public enum ACR122U_BuzzerControl
    {
        BuzzerOff = 0x00,
        BuzzerOnT1Cycle = 0x02,
        BuzzerOnT2Cycle = 0x01,
        BuzzerOnT1And2Cycle = 0x03
    }
}
