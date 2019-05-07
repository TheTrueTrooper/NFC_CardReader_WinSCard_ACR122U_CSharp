using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    class Block16UsableSpace : Block16
    {

        public Block16UsableSpace(BaseSector BaseSectorIn) : this(BaseSectorIn, Block16SubTypes.UsableSpace, false)
        {
            MemoryBytes = new byte[16];
        }

        internal Block16UsableSpace(BaseSector BaseSectorIn, Block16SubTypes SubTypeIn, bool ReadOnly) : base(BaseSectorIn, SubTypeIn, ReadOnly)
        {
            MemoryBytes = new byte[16];
        }

        public byte this[int i]
        {
            get
            {
                if (i > 15 || i < 0)
                    throw new IndexOutOfRangeException(StaticSharedErrors.IdexingError);
                if (AthenticatedA == true)
                {
                    Changed = true;
                    return MemoryBytes[i];
                }
                else
                    throw new Exception(StaticSharedErrors.AthenticationReadError);
            }
            set
            {
                if (i > 15 || i < 0)
                    throw new IndexOutOfRangeException(StaticSharedErrors.IdexingError);
                if (ReadOnly == false && AthenticatedB == true)
                    MemoryBytes[i] = value;
                else
                    throw new Exception(StaticSharedErrors.AthenticationWriteError);
            }
        }
    }
}
