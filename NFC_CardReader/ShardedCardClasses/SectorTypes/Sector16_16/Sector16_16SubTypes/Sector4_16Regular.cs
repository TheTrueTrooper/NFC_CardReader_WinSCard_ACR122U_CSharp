using NFC_CardReader.ShardedCardClasses.BlockTypes;
using NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.SectorTypes.Sector16_16NS
{
    public class Sector16_16Regular : Sector16_16
    {
        public Sector16_16Regular() : base(Sector16_16SubTypes.Regular)
        {
            MemoryBlocks = new BaseBlock[16]
            {
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16UsableSpace(this),
                new Block16KeyAndAccessBitsWithGeneralPurposeByte(this),
            };
        }
    }
}
