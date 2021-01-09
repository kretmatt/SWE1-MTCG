using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ServantFeature
{
    public class ArcherHierarchy:AServantHierarchy
    {
        public ArcherHierarchy()
        {
            servantStrengths=new List<EServantClass>
            {
                EServantClass.SABER,
                EServantClass.BERSERKER
            };
            servantWeaknesses = new List<EServantClass>
            {
                EServantClass.LANCER,
                EServantClass.BERSERKER
            };
            ServantClass = EServantClass.ARCHER;
        }
    }
}