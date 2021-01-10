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
    public class BattleHistoryRepositoryTests
    {
        private Mock<IBattleHistoryRepository> mockRepository;
        private List<BattleHistory> mockBattleHistories;
        private List<User> mockUsers;
        
        [SetUp]
        public void SetUp()
        {
            mockRepository=new Mock<IBattleHistoryRepository>();
            mockUsers = new List<User>
            {
                new User
                {
                    Id=1,
                    Bio = "Test Bio",
                    Username = "kretmatt"
                },
                new User
                {
                    Id=2,
                    Bio = "Test Bio 2 ",
                    Username = "testuser"
                }
            };
            mockBattleHistories = new List<BattleHistory>{   
                new BattleHistory
                {
                    Id=1,
                    BattleResult = EBattleResult.WIN,
                    PointChange = 27,
                    User = mockUsers[0]
                },
                new BattleHistory
                {
                    Id=2,
                    BattleResult = EBattleResult.LOSS,
                    PointChange = -27,
                    User = mockUsers[1]
                }
            };
        }
        
        [Test]
        public void CreateMock_CreateNewBattleHistoryForFirstMockUser()
        {
            //arrange
            mockRepository.Setup(mr => mr.Create(It.IsAny<User>(), It.IsAny<EBattleResult>(), It.IsAny<int>())).Returns((User user, EBattleResult battleResult, int pointChange) =>
            {
                //rough course of action:
                //check if user exists
                if (!mockUsers.Contains(user))
                    return 0;
                //Insert battle history
                BattleHistory battleHistory = new BattleHistory()
                {
                    Id = mockBattleHistories.Count+1,
                    BattleResult = battleResult,
                    User = user,
                    PointChange = pointChange
                };
                mockBattleHistories.Add(battleHistory);
                //Npgsql.ExecuteNonQuery returns integer of affected rows, here we check if the battlehistory object is in the collection
                if (mockBattleHistories.Contains(battleHistory))
                    return 1;
                return 0;
            });
            //act
            int result = mockRepository.Object.Create(mockUsers[0], EBattleResult.DRAW, 0);
            //assert
            mockRepository.Verify(mr=>mr.Create(mockUsers[0], EBattleResult.DRAW, 0));
            Assert.AreEqual(1,result);
        }
        [Test]
        public void ReadMock_ReturnSpecificBattleHistoryForFirstMockUser()
        {
            //arrange
            mockRepository.Setup(mr => mr.Read(It.IsAny<int>(), It.IsAny<User>())).Returns((int id, User user) =>
            {
                //rough course of action:
                //Search battlehistories for specific id and user
                BattleHistory battleHistory;
                battleHistory = mockBattleHistories.SingleOrDefault(bh=>bh.Id==id&&bh.User.Id==user.Id);
                return battleHistory;
            });
            //act
            BattleHistory bHistory = mockRepository.Object.Read(1, mockUsers[0]);
            //assert
            mockRepository.Verify(mr=>mr.Read(1,mockUsers[0]));
            Assert.AreEqual(mockBattleHistories[0],bHistory);
        }
        [Test]
        public void ReadAllMock_ReturnAllBattleHistoriesForFirstMockUser()
        {
            //arrange
            mockRepository.Setup(mr => mr.ReadAll(It.IsAny<User>())).Returns((User user) =>
            {
                //rough course of action:
                //Search battlehistories for specific user and return the collection
                return mockBattleHistories.Where(bh => bh.User.Id == user.Id).ToList();
            });
            //act
            List<BattleHistory> bHistory = mockRepository.Object.ReadAll(mockUsers[0]);
            //assert
            mockRepository.Verify(mr=>mr.ReadAll(mockUsers[0]));
            CollectionAssert.AreEqual(mockBattleHistories.Where(bh => bh.User.Id == mockUsers[0].Id).ToList(),bHistory);
        }
    }
}