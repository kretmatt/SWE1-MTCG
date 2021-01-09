using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class BerserkerHierarchy:AServantHierarchy
    {
        public BerserkerHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.SABER,
                EServantClass.ARCHER,
                EServantClass.LANCER,
                EServantClass.RIDER,
                EServantClass.ASSASSIN,
                EServantClass.CASTER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.SABER,
                EServantClass.ARCHER,
                EServantClass.LANCER,
                EServantClass.RIDER,
                EServantClass.ASSASSIN,
                EServantClass.CASTER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.BERSERKER;
        }
    }
}