using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122UManager
{
    public delegate void ACRCardStateChangeEventHandler(object sender, ACRCardStateChangeEventArg e);
    public delegate void ACRCardRemovedEventHandler(object sender, ACRCardRemovedEventArg e);
    public delegate void ACRCardDetectedEventHandler(object sender, ACRCardDetectedEventArg e);
    public delegate void ACRCardAcceptedCardScanEventHandler(object sender, ACRCardAcceptedCardScanEventArg e);
    public delegate void ACRCardRejectedCardScanEventHandler(object sender, ACRCardRejectedCardScanEventArg e);
}
