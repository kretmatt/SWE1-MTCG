namespace SWE1_MTCG
{
    public class AreaAction:ACardAction, IAreaAction
    {
        private IArea area;
        
        public AreaAction(double damage, EElementalAttributes elementalAttribute, ICard attacker, IArea area)
        {
            this.damage = damage;
            this.area = area;
            this.attacker = attacker;
            this.elementalAttribute = elementalAttribute;
        }


        public IArea ConstructArea()
        {
            return area;
        }
    }
}