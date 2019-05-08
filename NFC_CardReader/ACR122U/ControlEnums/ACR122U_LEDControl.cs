using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    /// <summary>
    /// Marks the T1 and T2 cycles for LED controlls
    /// </summary>
    [Flags]
    public enum ACR122U_LEDControl
    {
        /// <summary>
        /// Should the blinking light blink as Green
        /// </summary>
        GreenBlinkingMask = 0x80,
        /// <summary>
        /// Should the blinking light blink as Red
        /// </summary>
        RedBlinkingMask = 0x40,
        /// <summary>
        /// Should the blinking state be Green
        /// </summary>
        InitialGreenBlinkingState = 0x20,
        /// <summary>
        /// Should the blinking state be Green
        /// </summary>
        InitialRedBlinkingState = 0x10,
        /// <summary>
        /// Should the mask state be Green
        /// </summary>
        GreenLEDStateMask = 0x08,
        /// <summary>
        /// Should the mask state be Green
        /// </summary>
        RedLEDStateMask = 0x04,
        /// <summary>
        /// Should the Final state be Green
        /// </summary>
        GreenFinalState = 0x02,
        /// <summary>
        /// Should the Final state be Red
        /// </summary>
        RedFinalState = 0x01,
        /// <summary>
        /// Simply settes all of the markers off
        /// </summary>
        AllOff = 0x00,
        /// <summary>
        /// Simply settes all of the markers on
        /// </summary>
        AllOn = 0xFF
    }
}
