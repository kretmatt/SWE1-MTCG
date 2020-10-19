namespace SWE1_MTCG
{
    public class ElementalArea:IArea
    {
        private IElementalAttribute elementalAttribute;
        
        public ElementalArea(IElementalAttribute elementalAttribute)
        {
            this.elementalAttribute = elementalAttribute;
        }
        
        public ICardAction InfluenceBattle(ICardAction cardAction)
        {
            if(elementalAttribute.GetElementalAttribute()==cardAction.GetElementalAttribute())
                cardAction.SetDamage(cardAction.GetDamage()*2);
            if (elementalAttribute.CheckEffectiveness(cardAction.GetElementalAttribute()) < 1)
            {
                cardAction.SetElementalAttribute(elementalAttribute.GetElementalAttribute());
                cardAction.SetDamage(cardAction.GetDamage()*0.75);
            }
            return cardAction;
        }
    }
}