namespace SWE1_MTCG
{
    public class SpellCard:ICard
    {
        public string Name { get; set; }
        public double BaseDamage { get; set; }
        public IElementalAttribute ElementalAttribute { get; set; }

        public SpellCard()
        {
            
        }
        public SpellCard(string name, double baseDamage, IElementalAttribute elementalAttribute)
        {
            Name = name;
            BaseDamage = baseDamage;
            ElementalAttribute = elementalAttribute;
        }

        public string PrintCard()
        {
            throw new System.NotImplementedException();
        }

        public double ReceiveDamage(ICardAction cardAction)
        {
            return cardAction.GetDamage() * ElementalAttribute.CheckEffectiveness(cardAction.GetElementalAttribute());
        }

        public ICardAction UseCard()
        {
            return new AttackAction(BaseDamage,ElementalAttribute.GetElementalAttribute(),this);
        }
    }
}