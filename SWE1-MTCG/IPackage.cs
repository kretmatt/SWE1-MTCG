using System.Collections;
using System.Collections.Generic;

namespace SWE1_MTCG
{
    public interface IPackage
    {
        void AddCardToPackage(ICard card);
        void AddCardRange(IEnumerable<ICard> cards);
        IEnumerable<ICard> OpenPackage();
    }
}