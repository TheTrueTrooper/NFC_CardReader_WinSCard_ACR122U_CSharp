using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public enum ACR122U_LEDControl
    {
        GreenBlinkingMask = 0x80,
        RedBlinkingMask = 0x40,
        InitialGreenBlinkingState = 0x20,
        InitialRedBlinkingState = 0x10,
        GreenLEDStateMask = 0x08,
        RedLEDStateMask = 0x04,
        GreenFinalState = 0x02,
        RedFinalState = 0x01,
        AllOff = 0x00,
        AllOn = 0xFF
    }
}
