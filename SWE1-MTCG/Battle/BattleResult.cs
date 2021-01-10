using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.Battle
{
    public class BattleResult
    {
        public ACard Victor { get; set; }
        public ACard Attacker { get; set; }
        public ACard Defender { get; set; }
        public double AttackerDamage { get; set; }
        public double DefenderDamage { get; set; }
        public string BattleDescription { get; set; }
    }
}