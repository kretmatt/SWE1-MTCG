namespace SWE1_MTCG
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