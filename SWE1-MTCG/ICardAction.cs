namespace SWE1_MTCG
{
    public interface ICardAction
    {
        double GetDamage();
        void SetDamage(double damage);
        
        EElementalAttributes GetType();
        void SetType(EElementalAttributes elementalAttribute);

        ICard Attacker();
    }
}