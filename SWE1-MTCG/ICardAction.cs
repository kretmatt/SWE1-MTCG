namespace SWE1_MTCG
{
    public interface ICardAction
    {
        double GetDamage();
        void SetDamage(double damage);
        
        EElementalAttributes GetElementalAttribute();
        void SetElementalAttribute(EElementalAttributes elementalAttribute);

        ICard Attacker();
    }
}