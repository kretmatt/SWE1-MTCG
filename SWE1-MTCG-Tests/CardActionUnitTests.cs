using Moq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class CardActionUnitTests
    {
        private const double mockDamage = 100.0;
        private const EElementalAttributes mockElementalAttribute = EElementalAttributes.FIRE;
        private ICard mockCard;
        
        [SetUp]
        public void Setup()
        {
            mockCard = new MonsterCard();
        }
        
        [Test]
        public void GetDamageMock()
        {
            //arrange
            var mockCardAction = new Mock<ICardAction>();
            mockCardAction.Setup(cardAction => cardAction.GetDamage()).Returns(mockDamage);
            //act
            double tempDamage = mockCardAction.Object.GetDamage();
            //assert
            mockCardAction.Verify(cardAction=>cardAction.GetDamage());
        }

        [Test]
        public void SetDamageMock()
        {
            //arrange
            var mockCardAction = new Mock<ICardAction>();
            double setMockValue = 666;
            //act
            mockCardAction.Object.SetDamage(setMockValue);
            //assert
            mockCardAction.Verify(cardAction=>cardAction.SetDamage(setMockValue));
        }

        [Test]
        public void GetSetTypeMock()
        {
            //arrange
            var mockCardAction = new Mock<ICardAction>();
            mockCardAction.Setup(cardAction => cardAction.GetType()).Returns(mockElementalAttribute);
            //act
            EElementalAttributes tempType = mockCardAction.Object.GetType();
            //assert
            mockCardAction.Verify(cardAction=>cardAction.GetType());
        }

        [Test]
        public void SetTypeMock()
        {
            //arrange
            var mockCardAction = new Mock<ICardAction>();
            EElementalAttributes setMockValue = EElementalAttributes.WATER;
            //act
            mockCardAction.Object.SetType(setMockValue);
            //assert
            mockCardAction.Verify(cardAction=>cardAction.SetType(setMockValue));
        }

        [Test]
        public void GetAttackerMock()
        {
            //arrange
            var mockCardAction = new Mock<ICardAction>();
            mockCardAction.Setup(cardAction => cardAction.Attacker()).Returns(mockCard);
            //act
            ICard tempCard = mockCardAction.Object.Attacker();
            //assert
            mockCardAction.Verify(cardAction=> cardAction.Attacker());
        }
    }
}

/*
 double GetDamage();
        void SetDamage(double damage);
        
        EElementalAttributes GetType();
        void SetType(EElementalAttributes elementalAttribute);

        ICard Attacker();
    
 */