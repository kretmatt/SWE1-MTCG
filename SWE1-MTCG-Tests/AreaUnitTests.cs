using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class AreaUnitTests
    {
        
        
        private static IEnumerable<TestCaseData> InfluenceBattleTestCases
        {
            get
            {
                yield return new TestCaseData(new AttackAction(100, EElementalAttributes.FIRE, new SpellCard()),EElementalAttributes.WATER,75);
                yield return new TestCaseData(new AttackAction(100,EElementalAttributes.WATER, new MonsterCard()),EElementalAttributes.WATER,200);
            }
        }
        
        
        [Test]
        [TestCaseSource(nameof(InfluenceBattleTestCases))]
        public void InfluenceBattleTest(ICardAction cardAction, EElementalAttributes resultElementalAttribute,double damageResult)
        {
            //arrange
            IArea area = new ElementalArea(new WaterElementalAttribute());
            //act
            cardAction = area.InfluenceBattle(cardAction);
            //assert
            Assert.AreEqual(resultElementalAttribute,cardAction.GetElementalAttribute());
            Assert.AreEqual(damageResult,cardAction.GetDamage());
        }
    }
}