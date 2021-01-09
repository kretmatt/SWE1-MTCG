using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using SWE1_MTCG.Enums;
using SWE1_MTCG.ServantFeature;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class ServantHierarchyTests
    {
        private static IEnumerable<TestCaseData> CalculateServantAttackModifierTestCases
        {
            get
            {
                yield return new TestCaseData(new BerserkerHierarchy(),EServantClass.ARCHER,1);
                yield return new TestCaseData(new SaberHierarchy(),EServantClass.LANCER,0.5);
                yield return new TestCaseData(new ArcherHierarchy(),EServantClass.CASTER,1);
                yield return new TestCaseData(new LancerHierarchy(),EServantClass.SABER,2);
                yield return new TestCaseData(new RiderHierarchy(),EServantClass.ASSASSIN,2);
                yield return new TestCaseData(new AssassinHierarchy(),EServantClass.RIDER,0.5);
                yield return new TestCaseData(new CasterHierarchy(),EServantClass.BERSERKER,1);
            }
        }
        //For this test case not every possible combination is being tested, only some of the possible combinations are tested
        [Test]
        [TestCaseSource(nameof(CalculateServantAttackModifierTestCases))]
        public void CalculateServantAttackModifierTests(AServantHierarchy servantHierarchy, EServantClass servantClass, double result)
        {
            //arrange - Test data is already arranged in CalculateServantAttackModifierTestCases
            //act
            double attackModifier = servantHierarchy.CalculateServantAttackModifier(servantClass);
            //assert
            Assert.AreEqual(result, attackModifier);
        }
    }
}