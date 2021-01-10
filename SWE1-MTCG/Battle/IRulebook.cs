using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.Battle
{
    public interface IRulebook
    {
        BattleResult DetermineVictor(ACard attacker, ACard defender);
    }
}