using NFC_CardReader.WinSCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U.CardTypes.MifareClassic
{
    public class ACR122U_NTAG215_SmartCard : ACR122U_ISO14443TypeAStandard_SmartCard
    {
        protected internal ACR122U_NTAG215_SmartCard(WinSmartCard MakeFrom) : base(MakeFrom)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }

        protected internal ACR122U_NTAG215_SmartCard(WinSmartCardContext Parent, IntPtr Card) : base(Parent, Card)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }
    }
}
