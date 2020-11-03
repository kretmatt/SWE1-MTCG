namespace SWE1_MTCG
{
    public class AreaCard:ICard
    {
        public string Name { get; set; }
        public double BaseDamage { get; set; }
        public IElementalAttribute ElementalAttribute { get; set; }
        
        public EAreaType AreaType { get; set; }

        public AreaCard()
        {
            
        }
        public AreaCard(string name, double baseDamage, IElementalAttribute elementalAttribute,EAreaType areaType)
        {
            Name = name;
            BaseDamage = baseDamage;
            ElementalAttribute = elementalAttribute;
            AreaType = areaType;
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
            switch (AreaType)
            {
                default:
                    return new AreaAction(2, ElementalAttribute.GetElementalAttribute(), this,
                        new ElementalArea(ElementalAttribute));
                    break;
            }
        }
    }
}