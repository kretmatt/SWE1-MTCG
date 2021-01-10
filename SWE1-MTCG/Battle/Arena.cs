using System;
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
            
            if (battleFieldArea != null)
                battleResult = battleFieldArea.InfluenceBattle(battleResult);
            if (attackerTrap != null)
            {
                battleResult = attackerTrap.InfluenceBattle(battleResult, true);
                attackerTrap = null;
            }
            if (defenderTrap != null)
            {
                battleResult = defenderTrap.InfluenceBattle(battleResult, false);
                defenderTrap = null;
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
            
            return battleResult;
        }
    }
}