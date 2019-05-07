using NFC_CardReader.ShardedCardClasses.BlockTypes;
using NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.SectorTypes.Sector4_16NS
{
    public class Sector4_16Header : Sector4_16
    {
        public Sector4_16Header() : base(Sector4_16SubTypes.Header)
        {
            MemoryBlocks = new BaseBlock[4]
            {
                new Block16KeyAndAccessBitsWithGeneralPurposeByte(this),
                new Block16ApplicationDirectory(this),
                new Block16ApplicationDirectory(this),
                new Block16KeyAndAccessBitsWithGeneralPurposeByte(this),
            };
        }
    }
}
