using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DTOs
{
    public class TradingDeal
    {
        public int Id { get; set; }
        public ACard OfferedCard { get; set; }
        public int RequiredCoins { get; set; }
        public int MinDamage { get; set; }
        public ECardType CardType { get; set; }
        public User OfferingUser { get; set; }
    }
}