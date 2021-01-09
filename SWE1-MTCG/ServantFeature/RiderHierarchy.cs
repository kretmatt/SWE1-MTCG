using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class RiderHierarchy:AServantHierarchy
    {
        public RiderHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.CASTER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.ASSASSIN,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.RIDER;
        }
    }
}