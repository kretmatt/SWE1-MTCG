using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SWE1_MTCG;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class PackageRepositoryTests
    {
        private Mock<IPackageRepository> mockRepository;
        private User testUser;
        private List<Package> mockPackages;
        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IPackageRepository>();
            testUser = new User
            {
                Id=1,
                Username = "tester",
                Bio = "BIO",
                CardDeck = new List<ACard>(),
                CardStack = new List<ACard>(),
                Coins = 20
            };
            mockPackages = new List<Package>
            {
                new Package
                {
                    Id = 1,
                    Cards = new List<ACard>
                    {
                        new ServantCard
                        {
                            Id=1
                        },
                        new AreaCard
                        {
                            Id=2
                        },
                        new TrapCard
                        {
                            Id=3
                        },
                        new SpellCard
                        {
                            Id=4
                        },
                        new MonsterCard
                        {
                            Id=5
                        }
                    }
                }
            };
        }
        
        [Test]
        public void CreatePackageMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.CreatePackage(It.IsAny<List<ACard>>())).Returns((List<ACard> cards) =>
            {
                if (cards.Count != 5)
                    return 0;
                
                Package package = new Package
                {
                    Id = mockPackages.Count + 1,
                    Cards = new List<ACard>()
                };
                int affectedRows = 1;
                foreach (ACard card in cards)
                {
                    package.Cards.Add(card);
                    affectedRows += 1;
                }
                mockPackages.Add(package);
                return affectedRows;
            });
            //act
            mockRepository.Object.CreatePackage(mockPackages[0].Cards);
            //assert
            mockRepository.Verify(mr=>mr.CreatePackage(mockPackages[0].Cards));
            Assert.AreEqual(mockPackages[0].Cards.Count,mockPackages[1].Cards.Count);
        }
        [Test]
        public void OpenPackageMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.OpenPackage(It.IsAny<User>())).Returns((User user) =>
            {
                if (user == null)
                    return 0;
                Package package = mockPackages.FirstOrDefault();
                if (package == null)
                    return 0;

                int affectedRows = 0;

                foreach (ACard card in package.Cards)
                {
                    user.CardStack.Add(card);
                    affectedRows += 1;
                }

                user.Coins -= 4;
                mockPackages.Remove(package);
                affectedRows += 2;
                
                return affectedRows;
            });
            //act
            int openPackageResults = mockRepository.Object.OpenPackage(testUser);
            //assert
            mockRepository.Verify(mr=>mr.OpenPackage(testUser));
            Assert.AreEqual(5,testUser.CardStack.Count);
        }
        [Test]
        public void DeletePackageMock()
        {
            Package packageToBeDeleted = mockPackages[0];
            //arrange
            mockRepository.Setup(mr => mr.DeletePackage(It.IsAny<int>())).Returns((int id) =>
            {
                Package package = mockPackages.SingleOrDefault(mp => mp.Id == id);
                if (package == null)
                    return 0;
                mockPackages.Remove(package);
                if (mockPackages.Contains(package))
                    return 0;
                return 1;
            });
            //act
            int deleteResult = mockRepository.Object.DeletePackage(packageToBeDeleted.Id);
            //assert
            mockRepository.Verify(mr=>mr.DeletePackage(packageToBeDeleted.Id));
            Assert.AreEqual(1,deleteResult);
        }
    }
}