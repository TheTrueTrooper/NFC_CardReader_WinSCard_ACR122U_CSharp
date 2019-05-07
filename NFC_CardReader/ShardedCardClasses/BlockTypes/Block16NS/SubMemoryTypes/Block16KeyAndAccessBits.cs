using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    public abstract class Block16KeyAndAccessBits : Block16
    {
        const string GGPBIError = "This byte doesn't exist in this context as this is a key with a free Usable Byte. Are you sure you didn't want the UsableByte?";
        const string UBIError = "This byte doesn't exist in this context as this is a key with a General Purpose Byte. Are you sure you didn't want the GeneralPurposeByte?";
        const string OutIndexError = "That was not a valid index value. Please use KeyAndAccessBitsAccessIndexer enum for valid indexs.";
        const string KeySizeError = "Your keys size was not valid. It must be 6 bytes in length";
        const string AccessBitsSizeError = "Your Access Bits size was not valid. It must be 6 bytes in length";
        const string UBSizeError = "Your Usable Byte was not valid. It must be a single bytes in length";
        const string OutIndexErrorGetAppend = "\nAlso not that due to the dangers of thier use in writing accessors AccessBits and GeneralPurpousByte are not valid selections and should be called from fuctions with warrnings attached.";

        internal Block16KeyAndAccessBits(BaseSector BaseSectorIn, Block16SubTypes Type) : base(BaseSectorIn, Type, true)
        { }

        public byte[] this[KeyAndAccessBitsAccessIndexer Key]
        {
            get
            {
                byte[] Return; 
                switch (Key)
                {
                    case KeyAndAccessBitsAccessIndexer.KeyA:
                        Return = new byte[6];
                        Array.Copy(MemoryBytes, Return, 6);
                        return Return;
                    case KeyAndAccessBitsAccessIndexer.KeyB:
                        Return = new byte[6];
                        Array.Copy(MemoryBytes, 10, Return, 0, 6);
                        return Return;
                    case KeyAndAccessBitsAccessIndexer.AccessBits:
                        Return = new byte[3];
                        Array.Copy(MemoryBytes, 6, Return, 0, 3);
                        return Return;
                    case KeyAndAccessBitsAccessIndexer.UsableByte:
                        return GetUseableByte();
                    case KeyAndAccessBitsAccessIndexer.GeneralPurpousByte:
                        return GetGeneralPurposeByte();
                    default:
                        throw new IndexOutOfRangeException(OutIndexError);
                }
            }
            set
            {
                switch (Key)
                {
                    case KeyAndAccessBitsAccessIndexer.KeyA:
                        if (value.Length != 6)
                            throw new Exception(KeySizeError);
                        Array.Copy(value, MemoryBytes, 6);
                        break;
                    case KeyAndAccessBitsAccessIndexer.KeyB:
                        if (value.Length != 6)
                            throw new Exception(KeySizeError);
                        Array.Copy(value, 0, MemoryBytes, 10, 6);
                        break;
                    case KeyAndAccessBitsAccessIndexer.UsableByte:
                        SetUseableByte(value);
                        break;
                    default:
                        throw new IndexOutOfRangeException(OutIndexError + OutIndexErrorGetAppend);
                }
            }
        }

        virtual protected byte[] GetUseableByte()
        {
            throw new IndexOutOfRangeException(UBIError);
        }

        virtual protected void SetUseableByte(byte[] value)
        {
            throw new IndexOutOfRangeException(UBIError);
        }

        virtual protected byte[] GetGeneralPurposeByte()
        {
            throw new IndexOutOfRangeException(GGPBIError);
        }

        [Obsolete("Code is not obsolete; however, it is unsafe. Wrtting to this location could change or damage your cards info.")]
        virtual protected void SetGeneralPurposeByte(byte[] value)
        {
            if (value.Length != 1)
                throw new Exception(UBSizeError);
            throw new IndexOutOfRangeException(GGPBIError);
        }

        [Obsolete("Code is not obsolete; however, it is unsafe. Wrtting to this location could change or damage your card to the point of no access.")]
        void SetAccessBytes(byte[] value)
        {
            if (value.Length != 3)
                throw new Exception(AccessBitsSizeError);
            Array.Copy(value, 0, MemoryBytes, 6, 3);
        }

    }
}
