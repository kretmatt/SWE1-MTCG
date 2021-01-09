using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class AssassinHierarchy:AServantHierarchy
    {
        public AssassinHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.RIDER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.CASTER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.ASSASSIN;
        }
    }
}