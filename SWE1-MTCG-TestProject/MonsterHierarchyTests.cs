using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG.Enums;
using SWE1_MTCG.MonsterFeature;
using SWE1_MTCG.ServantFeature;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class MonsterHierarchyTests
    {

        private static IEnumerable<TestCaseData> CalculateMonsterAttackModifierTestCaseData
        {
            get
            {
                yield return new TestCaseData(new GoblinHierarchy(), EElementalType.FIRE, EMonsterType.DRAGON,0);
                yield return new TestCaseData(new OrkHierarchy(),EElementalType.GROUND, EMonsterType.WIZZARD,0);
                yield return new TestCaseData(new DragonHierarchy(),EElementalType.FIRE, EMonsterType.ELF,0);
            }
        }

        [Test]
        [TestCaseSource(nameof(CalculateMonsterAttackModifierTestCaseData))]
        public void CalculateMonsterAttackModifierTests(AMonsterHierarchy monsterHierarchy, EElementalType elementalType, EMonsterType monsterType, double result)
        {
            //arrange - data already arranged in CalculateMonsterAttackModifierTestCaseData
            //act
            double attackModifier = monsterHierarchy.CalculateMonsterAttackModifier(monsterType, elementalType);
            //assert
            Assert.AreEqual(result,attackModifier);
        }

        [Test]
        public void CalculateMixedBattleEnemyAttackModifier()
        {
            //arrange
            KrakenHierarchy kh = new KrakenHierarchy();
            ECardType spellType = ECardType.SPELL;
            EElementalType waterType = EElementalType.WATER;
            //act
            double attackModifier = kh.CalculateMixedBattleEnemyAttackModifier(spellType, waterType);
            //assert
            Assert.AreEqual(0,attackModifier);
        }

        [Test]
        public void CalculateMixedBattleOwnAttackModifier()
        {
            //arrange
            KnightHierarchy kh = new KnightHierarchy();
            ECardType spellType = ECardType.SPELL;
            EElementalType waterType = EElementalType.WATER;
            //act
            double attackModifier = kh.CalculateMixedBattleOwnAttackModifier(spellType, waterType);
            //assert
            Assert.AreEqual(0,attackModifier);
        }
        /*

        public virtual double CalculateMixedBattleOwnAttackModifier(ECardType cardType, EElementalType elementalType)

         */
    }
}