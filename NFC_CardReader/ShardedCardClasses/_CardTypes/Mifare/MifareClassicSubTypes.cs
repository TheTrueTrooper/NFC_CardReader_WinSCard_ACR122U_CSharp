using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses.Mifare
{
    enum MifareClassicSubTypes
    {
        [Description("Mifare Classic 1K Card")]
        Is1k,
        [Description("Mifare Classic 2K Card")]
        Is2K,
        [Description("Mifare Classic 4K Card")]
        Is4K
    }
}
