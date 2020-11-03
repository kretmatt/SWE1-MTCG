namespace SWE1_MTCG
{
    public class Wizzard:MonsterCard
    {
        public Wizzard(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength):base(name, baseDamage, elementalAttribute, armorPoints, strength)
        {
        }
        public override double ReceiveDamage(ICardAction cardAction)
        {
            double calculatedDamage = base.ReceiveDamage(cardAction);

            if (cardAction.Attacker().GetType() == typeof(Ork))
                calculatedDamage = 0;
            return calculatedDamage;
        }
    }
}