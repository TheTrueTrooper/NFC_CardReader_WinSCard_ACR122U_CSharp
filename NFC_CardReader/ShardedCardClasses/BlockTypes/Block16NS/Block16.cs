using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    /// <summary>
    /// Assumbed state of usable data Memory and the Base of all other Memories.
    /// </summary>
    public class Block16 : BaseBlock
    {
        /// <summary>
        /// The Sub Type of this location
        /// </summary>
        public Block16SubTypes SubType { get; private set; }


        internal Block16(BaseSector BaseSectorIn, Block16SubTypes SubTypeIn, bool ReadOnly) : base(BaseSectorIn, BlockTypes.block16, ReadOnly)
        {
            SubType = SubTypeIn;
            MemoryBytes = new byte[16];
        }
    }
}
