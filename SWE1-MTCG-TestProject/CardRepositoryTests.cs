using System.Collections.Generic;
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
    public class CardRepositoryTests
    {
        private Mock<ICardRepository> mockRepository;
        private List<ACard> mockCards;
        private User testUser;
        [SetUp]
        public void SetUp()
        {
            mockRepository=new Mock<ICardRepository>();
            mockCards = new List<ACard>
            {
                new TrapCard
                {
                    CardType =ECardType.TRAP,
                    Id = 1,
                    Damage = 10.5,
                    Description = "Just a simple pitfall trap.",
                    ElementalType = EElementalType.NORMAL,
                    Name = "Pitfall trap",
                    Uses = 3
                },
                new ServantCard
                {
                    CardType = ECardType.SERVANT,
                    Damage = 100,
                    Description = "Death doesn't even faze him anymore.",
                    ElementalType = EElementalType.FIRE,
                    Id = 2,
                    Name = "Arash",
                    ServantClass = EServantClass.ARCHER
                },
                new AreaCard
                {
                    CardType = ECardType.AREA,
                    Damage = 4,
                    Description = "A riverbank that will soon witness the horrors of battle.",
                    ElementalType = EElementalType.WATER,
                    Id = 3,
                    Name = "Riverbank",
                    Uses = 5
                },
                new MonsterCard
                {
                    CardType = ECardType.MONSTER,
                    Damage = 10,
                    Description = "Look! It's a goblin",
                    ElementalType = EElementalType.NORMAL,
                    Id = 4,
                    MonsterType = EMonsterType.GOBLIN,
                    Name = "Goblin"
                },
                new SpellCard
                {
                    CardType = ECardType.SPELL,
                    Damage = 10,
                    Description = "A fireball",
                    ElementalType = EElementalType.FIRE,
                    Id = 5,
                    Name = "Fireball"
                }
            };

            testUser = new User()
            {
                CardStack = new List<ACard>
                {
                    new TrapCard
                    {
                        Id=1
                    },
                    new ServantCard
                    {
                        Id=2
                    },
                    new AreaCard
                    {
                        Id=3
                    }
                }
            };

        }
        //Test only for card stack; the process of loading cards for specific users/packages is pretty much the same except for minor differences
        [Test]
        public void LoadCardsMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.LoadCardStack(It.IsAny<User>())).Returns((User user) =>
            {
                List<ACard> cards = new List<ACard>();
                foreach (ACard card in user.CardStack)
                {
                    cards.Add(mockCards.SingleOrDefault(c=>c.Id==card.Id));
                }
                return cards;
            });
            //act
            List<ACard> userCards = mockRepository.Object.LoadCardStack(testUser);
            //assert
            mockRepository.Verify(mr=>mr.LoadCardStack(testUser));
            Assert.AreEqual(3,userCards.Count);
        }
        //Test only for monster card; the process of creating cards is pretty much the same except some minor differences (different fields)
        [Test]
        public void CreateCardsMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.CreateMonsterCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(),
                It.IsAny<EElementalType>(), It.IsAny<EMonsterType>())).Returns(
                (string name, string description, double damage, EElementalType elementalType,
                    EMonsterType monsterType) =>
                {
                    if (mockCards.Count(c => c.Name == name) > 0)
                        return 0;

                    MonsterCard monsterCard = new MonsterCard()
                    {
                        Id = mockCards.Count + 1,
                        CardType = ECardType.MONSTER,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        MonsterType = monsterType,
                        Name = name
                    };
                    
                    mockCards.Add(monsterCard);
                    if (mockCards.Contains(monsterCard))
                        return 1;
                    return 0;
                });
            //act
            int failCreateAffectedRows =
                mockRepository.Object.CreateMonsterCard("Arash", "Fail create", 10, EElementalType.FIRE,
                    EMonsterType.ELF);
            int succeedCreateAffectedRows = mockRepository.Object.CreateMonsterCard("Fire elf", "It's a fire elf.", 7,
                EElementalType.FIRE, EMonsterType.ELF);
            //assert
            mockRepository.Verify(mr=>mr.CreateMonsterCard("Arash", "Fail create", 10, EElementalType.FIRE,
                EMonsterType.ELF));
            mockRepository.Verify(mr=>mr.CreateMonsterCard("Fire elf", "It's a fire elf.", 7,
                EElementalType.FIRE, EMonsterType.ELF));
            Assert.AreEqual(1,succeedCreateAffectedRows);
            Assert.AreEqual(0, failCreateAffectedRows);
        }
        [Test]
        public void DeleteCardMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Delete(It.IsAny<int>())).Returns((int id) =>
            {
                ACard card = mockCards.SingleOrDefault(c => c.Id == id);
                mockCards.Remove(card);
                if (mockCards.Contains(card))
                    return 0;
                return 1;
            });
            //act
            int affectedRows = mockRepository.Object.Delete(1);
            //assert
            mockRepository.Verify(mr=>mr.Delete(1));
            Assert.AreEqual(1,affectedRows);
        }
        [Test]
        public void ReadCardMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Read(It.IsAny<int>())).Returns((int id) =>
            {
                return mockCards.SingleOrDefault(c => c.Id == id);
            });
            //act
            ACard notExistingCard = mockRepository.Object.Read(12345);
            ACard servantCard = mockRepository.Object.Read(2);
            //assert
            mockRepository.Verify(mr=>mr.Read(12345));
            mockRepository.Verify(mr=>mr.Read(2));
            Assert.AreEqual(mockCards.SingleOrDefault(c=>c.Id==servantCard.Id),servantCard);
            Assert.IsNull(notExistingCard);
        }
        [Test]
        public void ReadAllCardsMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.ReadAll()).Returns(mockCards);
            //act
            List<ACard> retrievedCards = mockRepository.Object.ReadAll();
            //assert
            mockRepository.Verify(mr=>mr.ReadAll());
            CollectionAssert.AreEqual(mockCards, retrievedCards);
        }
    }
}