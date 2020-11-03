namespace SWE1_MTCG
{
    public class Kraken:MonsterCard
    {
        public Kraken(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength):base(name, baseDamage, elementalAttribute, armorPoints, strength)
        {
        }
        public override double ReceiveDamage(ICardAction cardAction)
        {
            double receivedDamage = base.ReceiveDamage(cardAction);

            if (cardAction.Attacker().GetType() == typeof(SpellCard))
                receivedDamage = 0;

            return receivedDamage;
        }
    }
}