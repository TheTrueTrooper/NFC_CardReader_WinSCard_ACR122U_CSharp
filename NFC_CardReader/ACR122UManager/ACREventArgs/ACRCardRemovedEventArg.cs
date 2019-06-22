using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122UManager
{
    public class ACRCardRemovedEventArg : ACRCardAcceptedCardScanEventArg
    {
        internal ACRCardRemovedEventArg(ACR122UManager EventsReader, ACRCardStateChangeEventArg State) : base(EventsReader, State)
        {
        }
    }
}
