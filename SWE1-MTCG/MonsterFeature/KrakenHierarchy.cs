using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public class KrakenHierarchy:AMonsterHierarchy
    {
        public KrakenHierarchy()
        {
            MonsterType = EMonsterType.KRAKEN;
            
            specificElementalMonsterWeaknesses = new List<SpecifcElementalMonsterCombination>();
            specificElementalCardTypeStrengths = new List<SpecificElementalCardTypeCombination>();
            specificElementalCardTypeWeaknesses = new List<SpecificElementalCardTypeCombination>();
            generalMonsterWeaknesses = new List<EMonsterType>();
            generalCardWeaknesses = new List<ECardType>();
            generalCardStrengths = new List<ECardType>
            {
                ECardType.SPELL
            };
        }
    }
}