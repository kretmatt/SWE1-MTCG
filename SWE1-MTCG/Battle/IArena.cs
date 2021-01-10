using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.Battle
{
    public interface IArena
    {
        BattleResult ConductBattle(ACard attacker, ACard defender, IRulebook rulebook);
    }
}