using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class GoblinHierarchy:AMonsterHierarchy
    {
        public GoblinHierarchy()
        {
            MonsterType = EMonsterType.GOBLIN;
            
            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>();
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>();
            generalMonsterWeaknesses = new List<EMonsterType>
            {
                EMonsterType.DRAGON
            };
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>();
        }
    }
}