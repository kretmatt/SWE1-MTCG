namespace SWE1_MTCG
{
    public class Elf:MonsterCard
    {
        
        public Elf(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength):base(name, baseDamage, elementalAttribute, armorPoints, strength)
        {
        }
        public override double ReceiveDamage(ICardAction cardAction)
        {
            double receivedDamage = base.ReceiveDamage(cardAction);

            if (cardAction.Attacker().GetType() == typeof(Dragon) &&
                this.ElementalAttribute.GetElementalAttribute() == EElementalAttributes.FIRE)
                receivedDamage = 0;

            return receivedDamage;
        }
    }
}