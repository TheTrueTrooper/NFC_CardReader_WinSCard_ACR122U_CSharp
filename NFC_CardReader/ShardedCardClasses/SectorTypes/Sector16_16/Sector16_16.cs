using NFC_CardReader.ShardedCardClasses.BlockTypes;
using NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.SectorTypes.Sector16_16NS
{
    public  class Sector16_16 : BaseSector
    {
        Sector16_16SubTypes Sector16_16SubTypes;

        internal Sector16_16(Sector16_16SubTypes Sector16_16SubTypesIn) : base(SectorTypes.Sector16_16)
        {
            Sector16_16SubTypes = Sector16_16SubTypesIn;
        }

    }
}
