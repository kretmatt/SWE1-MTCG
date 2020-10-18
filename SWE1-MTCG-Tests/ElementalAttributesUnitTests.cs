using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class ElementalAttributesUnitTests
    {
        private static IEnumerable<TestCaseData> CheckEffectivenessTestCases
        {
            get
            {
                yield return new TestCaseData(new FireElementalAttribute(),new WaterElementalAttribute(),2);
                yield return new TestCaseData(new FireElementalAttribute(), new NormalElementalAttribute(),0.5);
                yield return new TestCaseData(new WaterElementalAttribute(),new FireElementalAttribute(),0.5);
                yield return new TestCaseData(new WaterElementalAttribute(), new NormalElementalAttribute(),2);
                yield return new TestCaseData(new NormalElementalAttribute(), new WaterElementalAttribute(),0.5);
                yield return new TestCaseData(new NormalElementalAttribute(),new FireElementalAttribute(),2);
            }
        }
        
        [Test]
        [TestCaseSource(nameof(CheckEffectivenessTestCases))]
        public void TestCheckEffectiveness(IElementalAttribute firstAttribute,IElementalAttribute secondAttribute,double result)
        {
            //Assert
            Assert.AreEqual(result, firstAttribute.CheckEffectiveness(secondAttribute.GetType()));
        }
    }
}