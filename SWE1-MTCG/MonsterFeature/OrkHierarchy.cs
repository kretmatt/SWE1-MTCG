using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class OrkHierarchy:AMonsterHierarchy
    {
        public OrkHierarchy()
        {
            MonsterType = EMonsterType.ORK;
            
            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>();
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>();
            generalMonsterWeaknesses = new List<EMonsterType>
            {
                EMonsterType.WIZZARD
            };
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>();
        }
    }
}