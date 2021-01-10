namespace SWE1_MTCG.Battle
{
    public class Trap:ITrapBattleInfluencer
    {
        public double PureDamage { get; set; }
        
        public BattleResult InfluenceBattle(BattleResult battleResult, bool plantedByAttacker)
        {
            if (plantedByAttacker)
                battleResult.AttackerDamage += PureDamage;
            else
                battleResult.DefenderDamage += PureDamage;

            if (battleResult.AttackerDamage > battleResult.DefenderDamage)
                battleResult.Victor = battleResult.Attacker;
            else if (battleResult.DefenderDamage > battleResult.AttackerDamage)
                battleResult.Victor = battleResult.Defender;
            else
                battleResult.Victor = null;

            return battleResult;
        }
    }
}