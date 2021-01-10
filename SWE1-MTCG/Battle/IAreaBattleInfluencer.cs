using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.Battle
{
    public interface IAreaBattleInfluencer
    {
        BattleResult InfluenceBattle(BattleResult battleResult);
    }
}