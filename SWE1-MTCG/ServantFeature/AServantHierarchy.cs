using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class AServantHierarchy
    {
        protected List<EServantClass> servantWeaknesses;
        protected List<EServantClass> servantStrengths;
        public EServantClass ServantClass;
        public virtual double CalculateServantAttackModifier(EServantClass servantClass)
        {
            double attackModifier=1;
            if (servantWeaknesses.Contains(servantClass))
                attackModifier *= 0.5;
            if (servantStrengths.Contains(servantClass))
                attackModifier *= 2;
            return attackModifier;
        }
    }
}