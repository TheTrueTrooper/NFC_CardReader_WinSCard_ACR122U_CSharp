using NFC_CardReader.ShardedCardClasses.BlockTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.SectorTypes
{
    public abstract class BaseSector
    {
        SectorTypes SectorType;

        /// <summary>
        /// A marker to keep track of if you have athenticated
        /// </summary>
        public bool AthenticatedB { get; set; } = false;

        /// <summary>
        /// A marker to keep track of if you have athenticated
        /// </summary>
        public bool AthenticatedA { get; set; } = false;

        protected BaseBlock[] MemoryBlocks;

        public virtual BaseBlock this[int i]
        {
            get => MemoryBlocks[i];
        }

        internal BaseSector(SectorTypes SectorTypeIn)
        {
            SectorType = SectorTypeIn;
        }

        public static Func<string> ToStringExtOveride;

        public override string ToString()
        {
            if (ToStringExtOveride == null)
            {
                if (AthenticatedA || AthenticatedB)
                {
                    string Return = "";
                    int i = 0;
                    foreach (BaseBlock BB in MemoryBlocks)
                    {
                        Return += "Block# " + i + " : " + BB + "\n";
                        i += 1;
                    }
                    Return.Remove(Return.Count() - 1, 1);
                    return Return;
                }
                else
                    return "No Data and Not yet athenticated";
            }
            return ToStringExtOveride();
        }


    }
}
