using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class KnightHierarchy:AMonsterHierarchy
    {
        public KnightHierarchy()
        {
            MonsterType = EMonsterType.KNIGHT;

            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>();
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>{
                new SpecificElementalCardTypeCombination
                {
                    CardType = ECardType.SPELL,
                    ElementalType = EElementalType.WATER
                }
            };
            generalMonsterWeaknesses = new List<EMonsterType>();
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>();
        }
    }
}