using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    public class Block16KeyAndAccessBitsWithUsableByte : Block16KeyAndAccessBits
    {
        const string UBSizeError = "Your Usable Byte was not valid. It must be a single bytes in length";

        public Block16KeyAndAccessBitsWithUsableByte(BaseSector BaseSectorIn) : base(BaseSectorIn, Block16SubTypes.KeyAndAccessBitsWithUsableByte)
        { }

        protected override void SetUseableByte(byte[] value)
        {
            if (value.Length != 1)
                throw new Exception(UBSizeError);
            Array.Copy(value, 0, MemoryBytes, 9, 1);
        }

        protected override byte[] GetUseableByte()
        {
            byte[] Return = new byte[1];
            Array.Copy(MemoryBytes, 9, Return, 0, 1);
            return Return;
        }
    }
}
