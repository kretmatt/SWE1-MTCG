using NUnit.Framework;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class ElementalAttributesUnitTests
    {
        [Test]
        [TestCase(new FireElementalAttribute(), new WaterElementalAttribute(), 2)]
        [TestCase(new WaterElementalAttribute(), new FireElementalAttribute(), 0.5)]
        [TestCase(new NormalElementalAttribute(), new FireElementalAttribute(), 2)]
        [TestCase(new FireElementalAttribute(), new NormalElementalAttribute(), 0.5)]
        [TestCase(new WaterElementalAttribute(), new NormalElementalAttribute(), 2)]
        [TestCase(new NormalElementalAttribute(), new WaterElementalAttribute(), 0.5)]
        public void TestCheckEffectiveness(IElementalAttribute firstAttribute, IElementalAttribute secondAttribute, double result)
        {
            //Assert
            Assert.AreEqual(result, firstAttribute.CheckEffectiveness(secondAttribute));
        }
    }
}