using Moq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class AreaActionUnitTests
    {
        private IAreaAction areaAction;

        [SetUp]
        public void SetUp()
        {
            areaAction=new AreaAction(2, EElementalAttributes.WATER, new AreaCard(), new ElementalArea(new WaterElementalAttribute()));
        }
        
        [Test]
        public void ConstructAreaMock()
        {
            //arrange
            var mockAreaAction = new Mock<IAreaAction>();
            mockAreaAction.Setup(areaAction => areaAction.ConstructArea()).Returns(areaAction.ConstructArea());
            //act
            IArea tempArea = mockAreaAction.Object.ConstructArea();
            //assert
            mockAreaAction.Verify(areaAction=>areaAction.ConstructArea());
        }
    }
}