using NFC_CardReader.ShardedCardClasses.SectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ShardedCardClasses
{
    internal abstract class BaseCard
    {
        public CardTypes CardType { get; private set; }

        protected BaseSector[] MemorySectors;
            
        public virtual BaseSector this[int i]
        {
            get => MemorySectors[i];
        }

        internal BaseCard(CardTypes CardTypeIn)
        {
            CardType = CardTypeIn;
        }
    }
}
