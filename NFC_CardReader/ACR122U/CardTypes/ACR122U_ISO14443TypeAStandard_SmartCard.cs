using NFC_CardReader.WinSCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U.CardTypes
{
    public class ACR122U_ISO14443TypeAStandard_SmartCard : ACR122U_SmartCard
    {
        protected internal ACR122U_ISO14443TypeAStandard_SmartCard(WinSmartCard MakeFrom) : base(MakeFrom)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }

        protected internal ACR122U_ISO14443TypeAStandard_SmartCard(WinSmartCardContext Parent, IntPtr Card) : base(Parent, Card)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }

        #region ISO14443TypeAStandard Commands
        /// <summary>
        /// Gets the UID as bytes
        /// </summary>
        /// <param name="receivedUID">the UID</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes GetcardUIDBytes(out byte[] receivedUID)//only for mifare 1k cards
        {
            byte[] CommandAsBytes = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 };

            TransmitData(CommandAsBytes, out receivedUID);

            LastACRResultCode = RetrieveDataCodes(ref receivedUID);
            return LastACRResultCode;
        }

        #endregion
    }
}
