using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class ArenaUnitTests
    {

        private IArena arena;
        private static IEnumerable<TestCaseData> MonsterBattleTests
        {
            get
            {
                yield return new TestCaseData(StandardPackageFactory.highRarityPackage[2],StandardPackageFactory.highRarityPackage[3], StandardPackageFactory.highRarityPackage[3]);//goblin vs dragon -> dragon
                yield return new TestCaseData(StandardPackageFactory.standardPackage[1], StandardPackageFactory.highRarityPackage[3], StandardPackageFactory.standardPackage[1]);//fire elf vs dragon -> fire elf
                yield return new TestCaseData(StandardPackageFactory.testPackage[0], StandardPackageFactory.standardPackage[4], StandardPackageFactory.standardPackage[4]);//ork vs. wizzard -> wizzard
            }
        }
        
        private static IEnumerable<TestCaseData> MixedBattleTests
        {
            get
            {
                yield return new TestCaseData(StandardPackageFactory.highRarityPackage[1],StandardPackageFactory.standardPackage[2], StandardPackageFactory.standardPackage[2]);//knight vs. water spell -> water spell
                yield return new TestCaseData(StandardPackageFactory.highRarityPackage[4], StandardPackageFactory.standardPackage[2], StandardPackageFactory.highRarityPackage[4]);//kraken vs. spell -> kraken
            }
        }

        private static IEnumerable<TestCaseData> SpellBattleTests
        {
            get
            {
                yield return new TestCaseData(StandardPackageFactory.testPackage[1], StandardPackageFactory.standardPackage[2], StandardPackageFactory.testPackage[1]);//normal vs. water -> normal
                yield return new TestCaseData(StandardPackageFactory.testPackage[1],StandardPackageFactory.standardPackage[3],StandardPackageFactory.standardPackage[3]);//normal vs. fire -> fire
                yield return new TestCaseData(StandardPackageFactory.standardPackage[2],StandardPackageFactory.standardPackage[3],StandardPackageFactory.standardPackage[2]);//water vs. fire -> water
            }
        }

        [SetUp]
        public void Setup()
        {
            arena=new Arena();
        }
        
        [Test]
        [TestCaseSource(nameof(MonsterBattleTests))]
        public void MonsterBattleTest(ICard defender, ICard attacker, ICard winner)
        {
            //act
            ICard battleWinner = arena.DetermineVictor(attacker, defender);
            //assert
            Assert.AreEqual(winner,battleWinner);
        }

        [Test]
        [TestCaseSource(nameof(SpellBattleTests))]
        public void SpellBattleTest(ICard defender, ICard attacker, ICard winner)
        {
            //act
            ICard battleWinner = arena.DetermineVictor(attacker, defender);
            //assert
            Assert.AreEqual(winner,battleWinner);
        }
        
        [Test]
        [TestCaseSource(nameof(MixedBattleTests))]
        public void MixedBattleTest(ICard defender, ICard attacker, ICard winner)
        {
            //act
            ICard battleWinner = arena.DetermineVictor(attacker, defender);
            //assert
            Assert.AreEqual(winner,battleWinner);
        }

        [Test]
        public void ArenaAreaTest()
        {
            //arrange
            ICard firstWinner;
            ICard secondWinner;
            //act
            firstWinner = arena.DetermineVictor(StandardPackageFactory.standardPackage[0],StandardPackageFactory.highRarityPackage[0]);//goblin vs. sunny battlefield -> goblin
            secondWinner = arena.DetermineVictor(StandardPackageFactory.highRarityPackage[1],StandardPackageFactory.standardPackage[2]);//knight vs. water spell -> knight
            //assert
            Assert.AreEqual(StandardPackageFactory.standardPackage[0], firstWinner);
            Assert.AreEqual(StandardPackageFactory.highRarityPackage[1], secondWinner);
        }
    }
}