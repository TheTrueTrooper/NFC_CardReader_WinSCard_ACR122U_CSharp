using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes
{
    public abstract class BaseBlock
    {
        BaseSector BaseSector;
        /// <summary>
        /// The Memory
        /// </summary>
        protected byte[] MemoryBytes;

        /// <summary>
        /// For a change commit system
        /// </summary>
        public bool Changed { get; protected set; } = false;

        /// <summary>
        /// Should you be able to write to this sector
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// A marker to keep track of if you have athenticated
        /// </summary>
        public bool AthenticatedB { get => BaseSector.AthenticatedB; set => BaseSector.AthenticatedB = value; }

        /// <summary>
        /// A marker to keep track of if you have athenticated
        /// </summary>
        public bool AthenticatedA { get => BaseSector.AthenticatedB; set => BaseSector.AthenticatedB = value; }

        /// <summary>
        /// The Type of this location
        /// </summary>
        public BlockTypes Type { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubTypeIn"></param>
        internal BaseBlock(BaseSector BaseSectorIn, BlockTypes TypeIn, bool ReadOnlyIn = false)
        {
            BaseSector = BaseSectorIn;
            Type = TypeIn;
            ReadOnly = ReadOnlyIn;
        }

        public static Func<string> ToStringExtOveride;

        public override string ToString()
        {
            if (ToStringExtOveride == null)
            {
                if (AthenticatedA || AthenticatedB)
                    return BitConverter.ToString(MemoryBytes);
                else
                    return "No Data and Not yet athenticated";
            }
            return ToStringExtOveride();
        }
    }
    
}
