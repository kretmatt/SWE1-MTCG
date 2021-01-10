using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Battle
{
    public class Area:IAreaBattleInfluencer
    {
        public EElementalType ElementalType { get; set; }

        public BattleResult InfluenceBattle(BattleResult battleResult)
        {
            if (battleResult.Attacker.ElementalType == ElementalType)
                battleResult.AttackerDamage *= 2;
            if (battleResult.Defender.ElementalType == ElementalType)
                battleResult.DefenderDamage *= 2;

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