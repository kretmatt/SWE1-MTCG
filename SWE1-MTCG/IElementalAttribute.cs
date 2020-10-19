namespace SWE1_MTCG
{
    public interface IElementalAttribute
    {
        double CheckEffectiveness(EElementalAttributes attribute);
        EElementalAttributes GetElementalAttribute();
    }
}