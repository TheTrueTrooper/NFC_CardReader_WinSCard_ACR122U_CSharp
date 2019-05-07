using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.BlockTypes.Block16NS
{
    public enum Block16SubTypes
    {
        UsableSpace,
        ManufacturerData,
        ApplicationDirectory,
        KeyAndAccessBitsWithGeneralPurposeByte,
        KeyAndAccessBitsWithUsableByte,
    }
}
