namespace SWE1_MTCG
{
    public class BattleHistory
    {
        public int Id { get; set; }
        public User User { get; set; }
        public EBattleResult BattleResult { get; set; }
        public int PointChange { get; set; }
    }
}