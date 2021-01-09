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
    public class UserStatsRepositoryTests
    {
        private Mock<IUserStatsRepository> mockRepository;
        private List<UserStats> mockUserStats;
        private List<User> mockUsers;
        private List<BattleHistory> mockBattleHistories;
            
        [SetUp]
        public void SetUp()
        {
            mockRepository=new Mock<IUserStatsRepository>();
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
            mockUserStats = new List<UserStats>
            {
                new UserStats
                {
                    Points = 1000,
                    User = mockUsers[0],
                    WinLoseRatio = 1.0
                },
                new UserStats
                {
                    Points = 1000,
                    User = mockUsers[1],
                    WinLoseRatio = 1.0
                }
            };
        }
        [Test]
        public void ReadAllMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.ReadAll()).Returns(mockUserStats);
            //act
            List<UserStats> results = mockRepository.Object.ReadAll();
            //assert
            mockRepository.Verify(mr=>mr.ReadAll());
            CollectionAssert.AreEqual(mockUserStats, results);
        }
        [Test]
        public void ReadMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Read(It.IsAny<User>())).Returns((User user) =>
            {
                //rough course of action:
                //check if user exists
                if (!mockUsers.Contains(user))
                    return null;
                //return userstats object of specified user
                return mockUserStats.SingleOrDefault(us => us.User.Id == user.Id);
            });
            //act
            UserStats userStats = mockRepository.Object.Read(mockUsers[0]);
            //assert
            mockRepository.Verify(mr=>mr.Read(mockUsers[0]));
            Assert.AreEqual(mockUserStats[0],userStats);
        }
        [Test]
        public void UpdateMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Update(It.IsAny<User>())).Returns((User user) =>
            {
                //rough course of action:
                //Update function recalculates the users stats. The amount of wins, losses and the point changes are directly taken from the database. Here we get the data from a test collection
                //Check if user exists
                if (!mockUsers.Contains(user))
                    return 0;
                //Calculate current user stats
                int wins = mockBattleHistories.Count(bh => bh.User.Id == user.Id && bh.BattleResult.Equals(EBattleResult.WIN));
                int losses = mockBattleHistories.Count(bh => bh.User.Id == user.Id && bh.BattleResult.Equals(EBattleResult.LOSS));
                int currentpoints =1000 + mockBattleHistories.Where(bh => bh.User.Id == user.Id).Sum(bh=>bh.PointChange);
                double winloseratio;
                if(losses==0) 
                    winloseratio=wins;
                else 
                    winloseratio= (double) wins/ (double) losses;
                //update userstats
                if(!mockUserStats.Exists(us=>us.User.Id==user.Id))
                    return 0;

                UserStats userStats = mockUserStats.SingleOrDefault(us => us.User.Id == user.Id);
                userStats.Points = currentpoints;
                userStats.WinLoseRatio = winloseratio;
                
                return 1;
            });
            //act
            int result = mockRepository.Object.Update(mockUsers[0]);
            //assert
            Assert.AreEqual(1,result);
            mockRepository.Verify(mr=>mr.Update(mockUsers[0]));
            Assert.AreEqual(1000+mockBattleHistories.Where(bh=>bh.User.Id==mockUsers[0].Id).Sum(bh=>bh.PointChange),mockUserStats[0].Points);
        }
    }
}