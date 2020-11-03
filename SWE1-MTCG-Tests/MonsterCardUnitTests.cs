using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class MonsterCardUnitTests
    {

        private static IEnumerable<TestCaseData> MonsterBattleTestCases
        {
            get
            {
                yield return new TestCaseData(new Goblin("Absurdly strong goblin", 10, new WaterElementalAttribute(), 0,1000), new Dragon("Fafnir", 100,new FireElementalAttribute(), 20,7),0);
                yield return new TestCaseData(new Ork("Random ork", 25, new WaterElementalAttribute(), 5,3), new Wizzard("Merlin", 45,new NormalElementalAttribute(), 4,4),0);
                yield return new TestCaseData(new SpellCard("Aqua Sphere", 14, new WaterElementalAttribute()), new Knight("Gawain", 15,new FireElementalAttribute(), 20,9) ,-1);
                yield return new TestCaseData(new SpellCard("Eldritch blast",20,new NormalElementalAttribute()),new Kraken("Weak deep sea kraken", 8*5, new WaterElementalAttribute(), 8,4),0);
                yield return new TestCaseData(new Dragon("Fafnir", 100,new FireElementalAttribute(), 20,7), new Elf("Fire elf ranger", 10,new FireElementalAttribute(), 5,3),0);
            }
        }
        
        [Test]
        [TestCaseSource(nameof(MonsterBattleTestCases))]
        public void MonsterReceiveDamageTest(ICard attacker, ICard defender, double expectedDamage)
        {
            //arrange
            double damageDealt;
            //act
            damageDealt = defender.ReceiveDamage(attacker.UseCard());
            //assert
            Assert.AreEqual(expectedDamage,damageDealt);
        }
        
    }
}