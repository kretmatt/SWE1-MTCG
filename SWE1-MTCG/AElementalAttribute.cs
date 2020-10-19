using System.Collections.Generic;

namespace SWE1_MTCG
{
    public abstract class AElementalAttribute:IElementalAttribute
    {
        protected List<EElementalAttributes> strengths;
        protected List<EElementalAttributes> weaknesses;
        protected EElementalAttributes type;
        
        public double CheckEffectiveness(EElementalAttributes attribute)
        {
            double effectivenessModifier = 1;
            if (strengths.Contains(attribute))
                effectivenessModifier *= 0.5;
            if (weaknesses.Contains(attribute))
                effectivenessModifier *= 2;
            return effectivenessModifier;
        }

        public EElementalAttributes GetElementalAttribute()
        {
            return type;
        }
    }
}