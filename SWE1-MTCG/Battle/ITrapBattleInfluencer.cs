namespace SWE1_MTCG.Battle
{
    public interface ITrapBattleInfluencer
    {
        BattleResult InfluenceBattle(BattleResult battleResult, bool plantedByAttacker);
    }
}