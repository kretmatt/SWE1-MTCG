using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class DragonHierarchy:AMonsterHierarchy
    {
        public DragonHierarchy()
        {
            MonsterType = EMonsterType.DRAGON;
            
            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>
            {
                new SpecifcElementalMonsterCombination
                {
                    ElementalType = EElementalType.FIRE,
                    MonsterType = EMonsterType.ELF
                }
            };
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>();
            generalMonsterWeaknesses = new List<EMonsterType>();
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>();
        }
    }
}