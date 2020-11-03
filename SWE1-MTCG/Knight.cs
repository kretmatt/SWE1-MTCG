namespace SWE1_MTCG
{
    public class Knight:MonsterCard
    {
        public Knight(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength):base(name, baseDamage, elementalAttribute, armorPoints, strength)
        {
        }
        public override double ReceiveDamage(ICardAction cardAction)
        {
            double calculatedDamage = base.ReceiveDamage(cardAction);
            /* I am not really sure how to solve the insta-kill mechanic with inheritance alone. Currently I am taking advantage of the fact that ReceiveDamage can't return negative values. As soon as a negative value is detected, the opponent wins */
            if (cardAction.Attacker().GetType() == typeof(SpellCard) &&
                cardAction.GetElementalAttribute() == EElementalAttributes.WATER)
                calculatedDamage = -1;
            
            return calculatedDamage;
        }
    }
}