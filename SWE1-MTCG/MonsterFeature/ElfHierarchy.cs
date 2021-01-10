using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class ElfHierarchy:AMonsterHierarchy
    {
        public ElfHierarchy()
        {
            MonsterType = EMonsterType.ELF;
            
            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>();
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>();
            generalMonsterWeaknesses = new List<EMonsterType>();
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>();
        }
    }
}