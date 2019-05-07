using NFC_CardReader.ShardedCardClasses.SectorTypes;
using NFC_CardReader.ShardedCardClasses.SectorTypes.Sector16_16NS;
using NFC_CardReader.ShardedCardClasses.SectorTypes.Sector4_16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.Mifare
{
    class MifareClassic4K : BaseMifareClassic
    {
        internal MifareClassic4K() : base(MifareClassicSubTypes.Is4K)
        {
            MemorySectors = new BaseSector[40]
            {
                new Sector4_16Header(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Application(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector4_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular(),
                new Sector16_16Regular()
            };
        }

    }
}