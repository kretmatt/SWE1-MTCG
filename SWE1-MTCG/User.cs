using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public List<ACard> CardStack { get; set; }
        public List<ACard> CardDeck { get; set; }
        public int Coins { get; set; }
    }
}