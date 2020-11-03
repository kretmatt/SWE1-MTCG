namespace SWE1_MTCG
{
    public class Dragon:MonsterCard
    {

        public Dragon(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength):base(name, baseDamage, elementalAttribute, armorPoints, strength)
        {
        }
        public override double ReceiveDamage(ICardAction cardAction)
        {
            double calculatedDamage = base.ReceiveDamage(cardAction);

            if (cardAction.Attacker().GetType() == typeof(Goblin))
                calculatedDamage = 0;
            return calculatedDamage;
        }
    }
}