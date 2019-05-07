using NFC_CardReader.ShardedCardClasses.BlockTypes;
using NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.SectorTypes.Sector4_16NS
{
    public  class Sector4_16 : BaseSector
    {
        Sector4_16SubTypes Sector4_16SubTypes;

        internal Sector4_16(Sector4_16SubTypes Sector4_16SubTypesIn) : base(SectorTypes.Sector4_16)
        {
            Sector4_16SubTypes = Sector4_16SubTypesIn;
        }

    }
}
