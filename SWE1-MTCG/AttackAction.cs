namespace SWE1_MTCG
{
    public class AttackAction:ACardAction
    {
        public AttackAction(double damage, EElementalAttributes elementalAttribute, ICard attacker)
        {
            this.damage = damage;
            this.attacker = attacker;
            this.elementalAttribute = elementalAttribute;
        }
    }
}