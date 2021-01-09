using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class SaberHierarchy:AServantHierarchy
    {
        public SaberHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.LANCER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.ARCHER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.SABER;
        }
    }
}