using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class LancerHierarchy:AServantHierarchy
    {
        public LancerHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.ARCHER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.SABER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.LANCER;
        }
    }
}