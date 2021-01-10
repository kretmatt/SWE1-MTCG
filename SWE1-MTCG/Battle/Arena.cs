using System;
using System.Linq;
using System.Text;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Battle
{
    public class Arena:IArena
    {
        public IAreaBattleInfluencer battleFieldArea = null;
        public ITrapBattleInfluencer attackerTrap = null;
        public ITrapBattleInfluencer defenderTrap = null;
        
        public BattleResult ConductBattle(ACard attacker, ACard defender, IRulebook rulebook)
        {
            BattleResult battleResult = rulebook.DetermineVictor(attacker, defender);
            StringBuilder stringBuilder = new StringBuilder().Append(battleResult.BattleDescription);
            if (battleFieldArea != null)
            {
                battleResult = battleFieldArea.InfluenceBattle(battleResult);
                stringBuilder.Append(" The area influences the battle!");
            }
            if (attackerTrap != null)
            {
                battleResult = attackerTrap.InfluenceBattle(battleResult, true);
                attackerTrap = null;
                stringBuilder.Append("Attacker trap is triggered!");
            }
            if (defenderTrap != null)
            {
                battleResult = defenderTrap.InfluenceBattle(battleResult, false);
                defenderTrap = null;
                stringBuilder.Append("Defender trap is triggered!");
            }
                
            if(attacker.CardType==ECardType.TRAP)
                attackerTrap = new Trap{PureDamage = attacker.Damage*2};
            if(defender.CardType==ECardType.TRAP)
                defenderTrap=new Trap{PureDamage = defender.Damage*2};

            if (attacker.CardType == ECardType.AREA && defender.CardType == ECardType.AREA)
            {
                Random randomNumberGenerator = new Random();
                if(randomNumberGenerator.Next(0,2)==0)
                    battleFieldArea = new Area { ElementalType = defender.ElementalType};
                else
                    battleFieldArea = new Area { ElementalType = attacker.ElementalType};
            }else if (attacker.CardType != ECardType.AREA && defender.CardType == ECardType.AREA)
                battleFieldArea = new Area { ElementalType = defender.ElementalType};
            else if (attacker.CardType == ECardType.AREA && defender.CardType != ECardType.AREA)
                battleFieldArea = new Area { ElementalType = attacker.ElementalType};

            if (battleResult.Victor == null)
                stringBuilder.Append("There is no victor!");
            else
                stringBuilder.AppendFormat("The winner is {0}", battleResult.Victor.Name);
            battleResult.BattleDescription = stringBuilder.ToString();
            Console.WriteLine(battleResult.BattleDescription);
            return battleResult;
        }
    }
}