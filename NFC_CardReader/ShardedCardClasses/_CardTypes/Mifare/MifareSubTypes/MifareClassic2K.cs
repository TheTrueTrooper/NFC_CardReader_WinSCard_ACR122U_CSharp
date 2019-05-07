using NFC_CardReader.ShardedCardClasses.SectorTypes.Sector4_16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.Mifare
{
    class MifareClassic2K : BaseMifareClassic
    {
        internal MifareClassic2K() : base(MifareClassicSubTypes.Is1k)
        {
            MemorySectors = new Sector4_16[32]
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
                new Sector4_16Regular()
            };
        }

    }
}
