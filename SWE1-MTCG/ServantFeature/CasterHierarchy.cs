using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class CasterHierarchy:AServantHierarchy
    {
        public CasterHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.ASSASSIN,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.RIDER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.CASTER;
        }
    }
}