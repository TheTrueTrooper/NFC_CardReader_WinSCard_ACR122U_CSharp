using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    public class Block16ManufacturerData : Block16
    {

        public Block16ManufacturerData(BaseSector BaseSectorIn) : base(BaseSectorIn, Block16SubTypes.ApplicationDirectory, true)
        { }

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
        }

        [Obsolete("Code is not obsolete; however, it is unsafe. Wrtting to this location could change or damage your cards history info.")]
        public void UnsafeWrite(int i, byte WrteValue)
        {
            if (i > 15 || i < 0)
                throw new IndexOutOfRangeException(StaticSharedErrors.IdexingError);
            if (ReadOnly == false && AthenticatedB == true)
                MemoryBytes[i] = WrteValue;
            else
                throw new Exception(StaticSharedErrors.AthenticationWriteError);
        }
    }
}
