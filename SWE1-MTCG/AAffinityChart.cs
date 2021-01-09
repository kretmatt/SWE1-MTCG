using System.Collections.Generic;

namespace SWE1_MTCG
{
    public abstract class AAffinityChart:IAffinityChart
    {
        protected List<EElementalType> elementalWeaknesses;
        protected List<EElementalType> elementalStrengths;
        public EElementalType ElementalType;
        public virtual double CalculateElementalAttackModifier(EElementalType elementalType)
        {
            double attackModifier = 1;
            if (elementalWeaknesses.Contains(elementalType))
                attackModifier *= 2;
            if (elementalStrengths.Contains(elementalType))
                attackModifier *= 0.5;
            return attackModifier;
        }
    }
}