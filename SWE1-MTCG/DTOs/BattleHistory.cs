using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DTOs
{
    public class BattleHistory
    {
        public int Id { get; set; }
        public User User { get; set; }
        public EBattleResult BattleResult { get; set; }
        public int PointChange { get; set; }
    }
}