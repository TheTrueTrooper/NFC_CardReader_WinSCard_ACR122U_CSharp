using NFC_CardReader.ShardedCardClasses.SectorTypes;
using NFC_CardReader.ShardedCardClasses.SectorTypes.Sector4_16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.Mifare
{
    abstract class BaseMifareClassic : BaseCard
    {

        public MifareClassicSubTypes MifareClassicSubType { get; private set; }
        

        internal BaseMifareClassic(MifareClassicSubTypes MifareClassicSubTypeIn) : base(CardTypes.MifareClassic)
        {
            MifareClassicSubType = MifareClassicSubTypeIn;
        }

        public static Func<string> ToStringExtOveride = null;

        public override string ToString()
        {
            if (ToStringExtOveride == null)
            {
                string Return = MifareClassicSubType.ToString();
                int i = 0;
                foreach (BaseSector MS in MemorySectors)
                {
                    Return += "Sector " + i;
                    Return += MS;
                    i += 1;
                }
                Return.Remove(Return.Count() - 1, 1);
                return Return;
            }
            return ToStringExtOveride();
        }

    }
}
