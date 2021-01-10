using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Moq;
using NUnit.Framework;
using SWE1_MTCG;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class TradingDealRepositoryTests
    {
        private Mock<ITradingDealRepository> mockRepository;
        private User testUser;
        private User buyer;
        private List<User> mockUsers;
        private List<TradingDeal> mockTradingDeals;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<ITradingDealRepository>();
            testUser = new User
            {
                Id = 1,
                Username = "tester",
                Bio = "BIO",
                CardDeck = new List<ACard>
                {
                    new ServantCard
                    {
                        Id = 1
                    }
                },
                CardStack = new List<ACard>
                {
                    new ServantCard
                    {
                        Id = 1
                    },
                    new AreaCard
                    {
                        Id = 2
                    },
                    new MonsterCard
                    {
                        Id = 3
                    }
                },
                Coins = 20
            };
            buyer = new User
            {
                Id = 2,
                Username = "tester",
                Bio = "BIO",
                CardDeck = new List<ACard>(),
                CardStack = new List<ACard>
                {
                    new SpellCard
                    {
                        Id = 1,
                        Damage = 50,
                        CardType = ECardType.SPELL,
                        Description = "A weak fireball.",
                        Name = "Weak fireball",
                        ElementalType = EElementalType.FIRE
                    }
                },
                Coins = 20
            };
            
            mockTradingDeals = new List<TradingDeal>
            {
                new TradingDeal
                {
                    CardType = ECardType.SPELL,
                    Id = 1,
                    MinDamage = 50,
                    OfferedCard = testUser.CardStack[1],
                    OfferingUser = testUser,
                    RequiredCoins = 5
                }
            };
            mockUsers = new List<User>{testUser,buyer};
        }

        [Test]
        public void ReadAllMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.ReadAll()).Returns(mockTradingDeals);
            //act
            List<TradingDeal> tradingDealsResults = mockRepository.Object.ReadAll();
            //assert
            mockRepository.Verify(mr=>mr.ReadAll());
            CollectionAssert.AreEqual(mockTradingDeals, tradingDealsResults);
        }
        [Test]
        public void ReadMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Read(It.IsAny<int>())).Returns((int id) =>
            {
                TradingDeal tradingDeal = mockTradingDeals.SingleOrDefault(td => td.Id==id);
                return tradingDeal;
            });
            //act
            TradingDeal readTradingDeal = mockRepository.Object.Read(1);
            TradingDeal nullTradingDeal = mockRepository.Object.Read(10101010);
            //assert
            mockRepository.Verify(mr=>mr.Read(1));
            mockRepository.Verify(mr=>mr.Read(10101010));
            Assert.IsNull(nullTradingDeal);
            Assert.AreEqual(mockTradingDeals[0],readTradingDeal);
        }
        [Test]
        public void CreateMock()
        {
            //arrange
            TradingDeal tradingDeal = new TradingDeal();
            mockRepository.Setup(mr => mr.Create(It.IsAny<ACard>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<ECardType>(), It.IsAny<User>())).Returns(
                (ACard card, int requiredCoins, int minDamage, ECardType cardType, User user) =>
                {
                    if (requiredCoins < 0 ||
                        (user.CardDeck.Exists(c=>c.Id==card.Id) && user.CardStack.Count(c => c.Id == card.Id) <=
                            user.CardDeck.Count(c => c.Id == card.Id)) || !user.CardStack.Exists(c=>c.Id==card.Id))
                        return 0;

                    tradingDeal.Id = mockTradingDeals.Count + 1;
                    tradingDeal.CardType = cardType;
                    tradingDeal.MinDamage = minDamage;
                    tradingDeal.OfferingUser = user;
                    tradingDeal.RequiredCoins = requiredCoins;
                    tradingDeal.OfferedCard = card;
                    
                    mockTradingDeals.Add(tradingDeal);

                    if (mockTradingDeals.Contains(tradingDeal))
                        return 1;
                    return 0;
                });
            //act
            int affectedRowsTradeCardInCardDeck = mockRepository.Object.Create(testUser.CardStack[0], 20, 20, ECardType.AREA, testUser);
            int affectedRowsCreateTradingDeal =
                mockRepository.Object.Create(testUser.CardStack[1], 20, 20, ECardType.AREA, testUser);
            //assert
            mockRepository.Verify(mr=>mr.Create(testUser.CardStack[0], 20, 20, ECardType.AREA, testUser));
            mockRepository.Verify(mr=>mr.Create(testUser.CardStack[1], 20, 20, ECardType.AREA, testUser));
            Assert.AreEqual(1, affectedRowsCreateTradingDeal);
            Assert.AreEqual(0, affectedRowsTradeCardInCardDeck);
        }
        [Test]
        public void DeleteMock()
        {
            TradingDeal tradingDeal = mockTradingDeals[0];
            //arrange
            mockRepository.Setup(mr => mr.Delete(It.IsAny<int>(), It.IsAny<User>())).Returns((int id, User user) =>
            {
                TradingDeal td = mockTradingDeals.SingleOrDefault(t => t.Id == id&&t.OfferingUser.Id==user.Id);
                if (td == null)
                    return 0;
                mockTradingDeals.Remove(td);
                if (mockTradingDeals.Contains(td))
                    return 0;
                return 1;
            });
            //act
            int affectedRows = mockRepository.Object.Delete(1, testUser);
            //assert
            mockRepository.Verify(mr=>mr.Delete(1,testUser));
            Assert.AreEqual(1,affectedRows);
        }
        [Test]
        public void ConductTradeMock()
        {
            
            //arrange
            TradingDeal testDeal = mockTradingDeals[0];
            ACard buyerCard = buyer.CardStack[0];
            
            mockRepository.Setup(mr => mr.TradePossible(It.IsAny<TradingDeal>(), It.IsAny<User>())).Returns(
                (TradingDeal tradingDeal, User user) => (tradingDeal.OfferingUser.Id != user.Id) && tradingDeal.RequiredCoins <= user.Coins && user != null);
            
            mockRepository.Setup(mr => mr.TradePossible(It.IsAny<TradingDeal>(), It.IsAny<User>(), It.IsAny<ACard>()))
                .Returns(
                    (TradingDeal tradingDeal, User user, ACard card) => !user.CardDeck.Exists(c => c.Id == card.Id) && user.CardStack.Exists(c => c.Id == card.Id) && tradingDeal.OfferingUser.Id != user.Id);
            mockRepository.Setup(mr => mr.ConductTrade(It.IsAny<TradingDeal>(), It.IsAny<User>())).Returns(
                (TradingDeal tradingDeal, User user) =>
                {
                    if (!mockRepository.Object.TradePossible(tradingDeal, user))
                        return 0;

                    ACard cardToBeRemovedFromVendor =
                        tradingDeal.OfferingUser.CardStack.FirstOrDefault(c => c.Id == tradingDeal.OfferedCard.Id);

                    if (cardToBeRemovedFromVendor == null)
                        return 0;
                    int affectedRows = 0;
                    user.CardStack.Add(cardToBeRemovedFromVendor);
                    affectedRows += 1;
                    mockUsers.Single(u => u.Id == tradingDeal.OfferingUser.Id).CardStack
                        .Remove(cardToBeRemovedFromVendor);
                    affectedRows += 1;
                    mockTradingDeals.Remove(tradingDeal);
                    affectedRows += 1;
                    return affectedRows;
                });
            
            mockRepository.Setup(mr => mr.ConductTrade(It.IsAny<TradingDeal>(), It.IsAny<User>(), It.IsAny<ACard>())).Returns(
                (TradingDeal tradingDeal, User user, ACard card) =>
                {
                    if (!mockRepository.Object.TradePossible(tradingDeal, user,card))
                        return 0;

                    ACard cardToBeRemovedFromVendor =
                        tradingDeal.OfferingUser.CardStack.FirstOrDefault(c => c.Id == tradingDeal.OfferedCard.Id);

                    if (cardToBeRemovedFromVendor == null)
                        return 0;
                    int affectedRows = 0;
                    user.CardStack.Add(cardToBeRemovedFromVendor);
                    affectedRows += 1;
                    mockUsers.Single(u => u.Id == tradingDeal.OfferingUser.Id).CardStack
                        .Remove(cardToBeRemovedFromVendor);
                    affectedRows += 1;
                    mockUsers.Single(u => u.Id == tradingDeal.OfferingUser.Id).CardStack
                        .Add(card);
                    affectedRows += 1;
                    user.CardStack.Remove(card);
                    affectedRows += 1;
                    mockTradingDeals.Remove(tradingDeal);
                    affectedRows += 1;
                    return affectedRows;
                });
            //act -- remove third parameter for test of ConductTrade with only 2 parameters
            int tradeAffectedRowCount = mockRepository.Object.ConductTrade(testDeal, buyer,buyerCard);
            //assert -- comment the verifies which don't have the amount of parameters you want to test
            //mockRepository.Verify(mr=>mr.TradePossible(testDeal,buyer));
            //mockRepository.Verify(mr=>mr.ConductTrade(testDeal,buyer));
            
            mockRepository.Verify(mr=>mr.TradePossible(testDeal,buyer, buyerCard));
            mockRepository.Verify(mr=>mr.ConductTrade(testDeal,buyer,buyerCard ));
            Assert.IsTrue(tradeAffectedRowCount>=3);
        }
        [Test]
        public void TradePossibleMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.TradePossible(It.IsAny<TradingDeal>(), It.IsAny<User>())).Returns(
                (TradingDeal tradingDeal, User user) => (tradingDeal.OfferingUser.Id != user.Id) && tradingDeal.RequiredCoins <= user.Coins &&
                                                        user != null);

            mockRepository.Setup(mr => mr.TradePossible(It.IsAny<TradingDeal>(), It.IsAny<User>(), It.IsAny<ACard>()))
                .Returns(
                    (TradingDeal tradingDeal, User user, ACard card) => !user.CardDeck.Exists(c => c.Id == card.Id) && user.CardStack.Exists(c => c.Id == card.Id) && tradingDeal.OfferingUser.Id != user.Id);
            //act
            bool coinTradePossible = mockRepository.Object.TradePossible(mockTradingDeals[0], buyer);
            bool cardTradePossible =
                mockRepository.Object.TradePossible(mockTradingDeals[0], buyer, buyer.CardStack[0]);
            bool tradeWithSelf = mockRepository.Object.TradePossible(mockTradingDeals[0], testUser);
            //assert
            mockRepository.Verify(mr=>mr.TradePossible(mockTradingDeals[0], buyer, buyer.CardStack[0]));
            mockRepository.Verify(mr=>mr.TradePossible(mockTradingDeals[0], buyer));
            Assert.IsTrue(cardTradePossible);
            Assert.IsTrue(coinTradePossible);
            Assert.IsFalse(tradeWithSelf);
        }
    }
}