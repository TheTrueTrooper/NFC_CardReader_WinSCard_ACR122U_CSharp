using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U.CardTypes
{
    public enum ACR122U_SupportedRFCardTypes : byte
    {
        UnknownOrUnset = 0x00,
        MifareClassics = 0x01,
        NTAG215 = 0x02
    }
}
