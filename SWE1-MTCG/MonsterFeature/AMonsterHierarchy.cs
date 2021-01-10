using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.MonsterFeature
{
    public abstract class AMonsterHierarchy
    {
        protected List<EMonsterType> generalMonsterWeaknesses;
        protected List<SpecifcElementalMonsterCombination> specificElementalMonsterWeaknesses;

        protected List<ECardType> generalCardStrengths;
        protected List<SpecificElementalCardTypeCombination> specificElementalCardTypeStrengths;
        
        protected List<ECardType> generalCardWeaknesses;
        protected List<SpecificElementalCardTypeCombination> specificElementalCardTypeWeaknesses;

        public EMonsterType MonsterType;
        
        public virtual double CalculateMonsterAttackModifier(EMonsterType monsterType,
            EElementalType elementalType)
        {
            double attackModifier = 1;
            if (generalMonsterWeaknesses.Contains(monsterType))
                attackModifier *= 0;
            else if (specificElementalMonsterWeaknesses.Exists(w =>
                w.ElementalType == elementalType && w.MonsterType == monsterType))
                attackModifier *= 0;
            return attackModifier;
        }

        public virtual double CalculateMixedBattleEnemyAttackModifier(ECardType cardType, EElementalType elementalType)
        {
            double attackmodifier = 1;
            if (generalCardStrengths.Contains(cardType))
                attackmodifier *= 0;
            else if (specificElementalCardTypeStrengths.Exists(s =>
                s.CardType == cardType && s.ElementalType == elementalType))
                attackmodifier *= 0;
            return attackmodifier;
        }

        public virtual double CalculateMixedBattleOwnAttackModifier(ECardType cardType, EElementalType elementalType)
        {
            double attackmodifier = 1;
            if (generalCardWeaknesses.Contains(cardType))
                attackmodifier *= 0;
            else if (specificElementalCardTypeWeaknesses.Exists(s =>
                s.CardType == cardType && s.ElementalType == elementalType))
                attackmodifier *= 0;
            return attackmodifier;
        }
    }
}