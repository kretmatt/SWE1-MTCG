using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG;
using SWE1_MTCG.ElementalAffinities;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class ElementalAffinitiesTests
    {
        private static IEnumerable<TestCaseData> CalculateElementalAttackModifierTestCases
        {
            get
            {
                yield return new TestCaseData(new WaterAffinityChart(), EElementalType.ELECTRIC,2);
                yield return new TestCaseData(new WaterAffinityChart(), EElementalType.NORMAL,2);
                yield return new TestCaseData(new WaterAffinityChart(), EElementalType.FIRE,0.5);
                yield return new TestCaseData(new WaterAffinityChart(), EElementalType.GROUND,0.5);
                yield return new TestCaseData(new WaterAffinityChart(), EElementalType.WATER,1);
                
                yield return new TestCaseData(new NormalAffinityChart(), EElementalType.FIRE,2);
                yield return new TestCaseData(new NormalAffinityChart(), EElementalType.NORMAL,1);
                yield return new TestCaseData(new NormalAffinityChart(),EElementalType.ELECTRIC,1);
                yield return new TestCaseData(new NormalAffinityChart(), EElementalType.WATER,0.5);
                yield return new TestCaseData(new NormalAffinityChart(), EElementalType.GROUND,1);
                
                yield return new TestCaseData(new FireAffinityChart(), EElementalType.FIRE,1);
                yield return new TestCaseData(new FireAffinityChart(), EElementalType.NORMAL,0.5);
                yield return new TestCaseData(new FireAffinityChart(), EElementalType.ELECTRIC,1);
                yield return new TestCaseData(new FireAffinityChart(), EElementalType.WATER,2);
                yield return new TestCaseData(new FireAffinityChart(), EElementalType.GROUND,1);
                
                yield return new TestCaseData(new GroundAffinityChart(), EElementalType.FIRE,1);
                yield return new TestCaseData(new GroundAffinityChart(), EElementalType.NORMAL,1);
                yield return new TestCaseData(new GroundAffinityChart(), EElementalType.ELECTRIC,0.5);
                yield return new TestCaseData(new GroundAffinityChart(), EElementalType.WATER,2);
                yield return new TestCaseData(new GroundAffinityChart(), EElementalType.GROUND,1);
                
                yield return new TestCaseData(new ElectricAffinityChart(), EElementalType.FIRE,1);
                yield return new TestCaseData(new ElectricAffinityChart(), EElementalType.NORMAL,1);
                yield return new TestCaseData(new ElectricAffinityChart(), EElementalType.ELECTRIC,1);
                yield return new TestCaseData(new ElectricAffinityChart(), EElementalType.WATER,0.5);
                yield return new TestCaseData(new ElectricAffinityChart(), EElementalType.GROUND,2);
            }
        }

        [Test]
        [TestCaseSource(nameof(CalculateElementalAttackModifierTestCases))]
        public void CalculateElementalAttackModifierTests(AAffinityChart elementalAffinityChart, EElementalType secondAttribute, double expectedResult)
        {
            //arrange - Test case data is already arranged in collection CalculateElementalAttackModifierTestCases
            //act
            double attackModifier = elementalAffinityChart.CalculateElementalAttackModifier(secondAttribute);
            //assert
            Assert.AreEqual(expectedResult,attackModifier);
        }
        
        
    }
}