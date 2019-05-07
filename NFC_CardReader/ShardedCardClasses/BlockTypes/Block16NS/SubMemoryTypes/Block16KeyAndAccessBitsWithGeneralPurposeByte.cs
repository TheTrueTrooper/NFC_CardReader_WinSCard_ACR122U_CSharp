using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    public class Block16KeyAndAccessBitsWithGeneralPurposeByte : Block16KeyAndAccessBits
    {
        const string GGPBSizeError = "Your General Purpous Byte size was not valid. It must be a single byte bytes in length";
        public Block16KeyAndAccessBitsWithGeneralPurposeByte(BaseSector BaseSectorIn) : base(BaseSectorIn, Block16SubTypes.KeyAndAccessBitsWithGeneralPurposeByte)
        { }

        [Obsolete("Code is not obsolete; however, it is unsafe. Wrtting to this location could change or damage your cards info.")]
        protected override void SetGeneralPurposeByte(byte[] value)
        {
            if (value.Length != 1)
                throw new Exception(GGPBSizeError);
            Array.Copy(value, 0, MemoryBytes, 9, 1);
        }

        protected override byte[] GetGeneralPurposeByte()
        {
            byte[] Return = new byte[1];
            Array.Copy(MemoryBytes, 9, Return, 0, 1);
            return Return;
        }
    }
}
