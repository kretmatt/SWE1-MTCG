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

        public EElementalAttributes GetType()
        {
            return elementalAttribute;
        }

        public void SetType(EElementalAttributes elementalAttribute)
        {
            this.elementalAttribute = elementalAttribute;
        }

        public ICard Attacker()
        {
            return attacker;
        }
    }
}