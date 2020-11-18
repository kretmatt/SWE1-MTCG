using System.Collections.Generic;

namespace SWE1_MTCG
{
    //DTO -> no unit tests
    public class User
    {
        public int Coins { get; set; }
        public List<ICard> CardStack { get; set; }
        public List<ICard> CardDeck { get; set; }
        public List<ICard> CurrentlyUnavailableCards { get; set; }
        public string Name { get; set; }
        public int ELOScore { get; set; }
    }
}