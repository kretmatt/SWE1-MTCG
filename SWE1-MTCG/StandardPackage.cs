using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class StandardPackage:IPackage
    {
        private List<ICard> cards;

        public StandardPackage()
        {
            cards=new List<ICard>();
        }
        
        public void AddCardToPackage(ICard card)
        {
            if(cards.Count<=5)
                cards.Add(card);
        }

        public void AddCardRange(IEnumerable<ICard> cards)
        {
            foreach (ICard card in cards)
            {
                if(this.cards.Count<=5)
                    this.cards.Add(card);
            }
        }

        public IEnumerable<ICard> OpenPackage()
        {
            return cards;
        }
    }
}