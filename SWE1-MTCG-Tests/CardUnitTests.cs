using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class CardUnitTests
    {

        private const string mockPrintCardString = "MockMockMock";
        private ICardAction attackAction;
        private static IEnumerable<TestCaseData> ReceiveDamageTestCases
        {
            get
            {
                yield return new TestCaseData(new MonsterCard("AttackMonster",10,new FireElementalAttribute(), 0,2),new MonsterCard("DefendMomster",0,new WaterElementalAttribute(), 10,0),10);
                yield return new TestCaseData(new MonsterCard("AttackMonster",10,new FireElementalAttribute(), 0,2), new SpellCard("Water pulse",20,new WaterElementalAttribute()),10);
                yield return new TestCaseData(new SpellCard("Water pulse",20,new WaterElementalAttribute()),new MonsterCard("AttackMonster",10,new FireElementalAttribute(), 0,2),40);
            }
        }
        [SetUp]
        public void Setup()
        {
            attackAction = new AttackAction(10, EElementalAttributes.FIRE, new MonsterCard());
        }
        
        [Test]
        [TestCaseSource(nameof(ReceiveDamageTestCases))]
        public void ReceiveDamageTest(ICard attacker, ICard defender, double expectedReceivedDamage)
        {
            Assert.AreEqual(expectedReceivedDamage,defender.ReceiveDamage(attacker.UseCard()));
        }

        [Test]
        public void UseCardMock()
        {
            //arrange
            var mockCard = new Mock<ICard>();
            mockCard.Setup(card => card.UseCard()).Returns(attackAction);
            //act
            ICardAction tempCardAction = mockCard.Object.UseCard();
            //assert
            mockCard.Verify(card=>card.UseCard());
        }

        [Test]
        public void PrintCardTest()
        {
            //arrange
            var mockCard = new Mock<ICard>();
            mockCard.Setup(card => card.PrintCard()).Returns(mockPrintCardString);
            //act 
            string tempCartPrintString = mockCard.Object.PrintCard();
            //assert
            mockCard.Verify(card=>card.PrintCard());
        }
    }
}