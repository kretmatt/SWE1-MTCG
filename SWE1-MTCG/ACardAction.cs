namespace SWE1_MTCG
{
    public abstract class ACardAction:ICardAction
    {
        protected double damage;
        protected EElementalAttributes elementalAttribute;
        protected ICard attacker;

        public double GetDamage()
        {
            return damage;
        }

        public void SetDamage(double damage)
        {
            this.damage = damage;
        }

        public EElementalAttributes GetElementalAttribute()
        {
            return elementalAttribute;
        }

        public void SetElementalAttribute(EElementalAttributes elementalAttribute)
        {
            this.elementalAttribute = elementalAttribute;
        }

        public ICard Attacker()
        {
            return attacker;
        }
    }
}