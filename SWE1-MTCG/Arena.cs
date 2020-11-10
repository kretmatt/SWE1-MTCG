using System.Runtime.InteropServices.WindowsRuntime;

namespace SWE1_MTCG
{
    public class Arena:IArena
    {
        private IArea area;
        
        public ICard DetermineVictor(ICard attacker, ICard defender)
        {
            double attackerReceivedDamage, defenderReceivedDamage;
            
            if (attacker.GetType() == typeof(AreaCard))
            {
                area=((AreaAction) attacker.UseCard()).ConstructArea();
            }

            if (defender.GetType() == typeof(AreaCard))
            {
                area=((AreaAction) defender.UseCard()).ConstructArea();
            }

            if (attacker.GetType() == typeof(SpellCard) && defender.GetType() == typeof(SpellCard))
            {
                if (area != null)
                    return DetermineSpellBattleVictor(area.InfluenceBattle(defender.UseCard()),
                        area.InfluenceBattle(attacker.UseCard()));
                return DetermineSpellBattleVictor(defender.UseCard(), attacker.UseCard());
            }

            if (area != null)
            {
                defenderReceivedDamage = defender.ReceiveDamage(area.InfluenceBattle(attacker.UseCard()));
                attackerReceivedDamage = attacker.ReceiveDamage(area.InfluenceBattle(defender.UseCard()));
            }
            else
            {
                defenderReceivedDamage = defender.ReceiveDamage(attacker.UseCard());
                attackerReceivedDamage = attacker.ReceiveDamage(defender.UseCard());
            }

            if (defenderReceivedDamage < 0 && defenderReceivedDamage < attackerReceivedDamage)
                return attacker;
            if (attackerReceivedDamage < 0)
                return defender;

            if (attackerReceivedDamage < defenderReceivedDamage)
                return attacker;
            return defender;
        }

        private ICard DetermineSpellBattleVictor(ICardAction defenderAction, ICardAction attackerAction)
        {
            double attackerModifier, defenderModifier;
            attackerModifier = attackerAction.Attacker().ReceiveDamage(defenderAction);
            defenderModifier = defenderAction.Attacker().ReceiveDamage(attackerAction);

            if (attackerModifier < defenderModifier)
                return attackerAction.Attacker();
            return defenderAction.Attacker();
        }
    }
}