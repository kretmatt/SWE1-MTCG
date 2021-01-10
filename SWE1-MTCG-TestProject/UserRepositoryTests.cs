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
    public class UserRepositoryTests
    {
        private Mock<IUserRepository> mockRepository;
        private List<User> mockUsers;
        private List<UserStats> mockUserStats;
        
        [SetUp]
        public void SetUp()
        {
         mockRepository = new Mock<IUserRepository>();
         mockUsers = new List<User>
         {
             new User
             {
                 Id=1,
                 Bio = "Test Bio",
                 Username = "kretmatt",
                 Coins = 20,
                 CardDeck = new List<ACard>(),
                 CardStack = new List<ACard>
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
                         Id=3
                     }
                 }
             },
             new User
             {
                 Id=2,
                 Bio = "Test Bio 2 ",
                 Username = "testuser",
                 Coins = 0,
                 CardDeck = new List<ACard>(),
                 CardStack = new List<ACard>()
             }
         };
         mockUserStats = new List<UserStats>();
        }
        [Test]
        public void CreateMock()
        {
            //arrange
            string newBio = "Hey!";
            string newUsername = "fritzfranz";
            string newPassword = "123456";
            mockRepository.Setup(mr => mr.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string username, string password, string bio) =>
                {
                    //rough course of action: check if username is unique. If it is, create user and create userstats afterwards
                    if (mockUsers.Exists(mu => mu.Username == username))
                        return 0;

                    User user = new User
                    {    
                        Id = mockUsers.Count+1,
                        Bio = bio,
                        Username = username
                    };
                    // User DTO doesn't have password, the password is directly saved in the database
                    mockUsers.Add(user);

                    if (!mockUsers.Contains(user))
                        return 0;

                    UserStats userStats = new UserStats
                    {
                        User = user,
                        Points = 1000,
                        WinLoseRatio = 0
                    };
                    mockUserStats.Add(userStats);

                    if (!mockUserStats.Contains(userStats))
                        return 0;
                    return 2;
                });
            //act
            int result = mockRepository.Object.Create(newUsername, newPassword, newBio);
            //assert
            mockRepository.Verify(mr=>mr.Create(newUsername, newPassword, newBio));
            Assert.AreEqual(2,result);
            Assert.AreEqual(3,mockUsers.Count);
        }
        [Test]
        public void UpdateMock()
        {
            //arrange
            string updateBio = "Hey!";
            string updateUsername = "fritzfranz";
            mockRepository.Setup(mr => mr.Update(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                (User user, string username, string bio) =>
                {
                    //rough course of action: check if username is unique. If it is, update user
                    if (mockUsers.Exists(mu => mu.Username == username))
                        return 0;

                    User wantedUser = mockUsers.SingleOrDefault(mu => mu.Id == user.Id);

                    if (wantedUser == null)
                        return 0;

                    wantedUser.Bio = bio;
                    wantedUser.Username = username;

                    return 1;
                });
            //act
            int result = mockRepository.Object.Update(mockUsers[0], updateUsername, updateBio);
            //assert
            mockRepository.Verify(mr=>mr.Update(mockUsers[0],updateUsername,updateBio));
            Assert.AreEqual(1,result);
            Assert.AreEqual(mockUsers[0].Username,updateUsername);
        }
        [Test]
        public void DeleteMock()
        {
            //arrange
            string wantedUsername = "kretmatt";
            int wantedId = 2;
            mockRepository.Setup(mr => mr.Delete(It.IsAny<string>())).Returns((string username) =>
            {
                // delete user with specified username(unique)
                User user = mockUsers.SingleOrDefault(mu => mu.Username == username);
                mockUsers.Remove(user);
                if (mockUsers.Contains(user))
                    return 0;
                return 1;
            });
            mockRepository.Setup(mr => mr.Delete(It.IsAny<int>())).Returns((int id) =>
            {
                // delete user with specified id (unique)
                User user = mockUsers.SingleOrDefault(mu => mu.Id == id);
                mockUsers.Remove(user);
                if (mockUsers.Contains(user))
                    return 0;
                return 1;
            });
            //act
            int deleteFirstUser = mockRepository.Object.Delete(wantedUsername);
            int deleteSecondUser = mockRepository.Object.Delete(wantedId);
            //assert
            mockRepository.Verify(mr=>mr.Delete(wantedId));
            mockRepository.Verify(mr=>mr.Delete(wantedUsername));
            Assert.AreEqual(1,deleteFirstUser);
            Assert.AreEqual(1, deleteSecondUser);
        }
        [Test]
        public void ReadMock()
        {
            //arrange
            string wantedUsername = "kretmatt";
            int wantedId = 2;
            mockRepository.Setup(mr => mr.Read(It.IsAny<int>())).Returns((int id) =>
            {
                // return user with specified id or null if wanted user does not exist
                User user = mockUsers.SingleOrDefault(mu=>mu.Id==id);
                return user;
            });
            mockRepository.Setup(mr => mr.Read(It.IsAny<string>())).Returns((string username) =>
            {
                //return user with specified username or return null if wanted user does not exist
                User user = mockUsers.SingleOrDefault(mu => mu.Username == username);
                return user;
            });
            //act
            User firstUser = mockRepository.Object.Read(wantedUsername);
            User secondUser = mockRepository.Object.Read(wantedId);
            //assert
            mockRepository.Verify(mr=>mr.Read(wantedId));
            mockRepository.Verify(mr=>mr.Read(wantedUsername));
            Assert.AreEqual(mockUsers[0],firstUser);
            Assert.AreEqual(mockUsers[1],secondUser);
        }
        [Test]
        public void SetCardDeckMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.SetCardDeck(It.IsAny<List<ACard>>(), It.IsAny<User>())).Returns(
                (List<ACard> cards, User user) =>
                {
                    if (user == null)
                        return 0;

                    foreach (ACard card in cards)
                    {
                        if ((!user.CardStack.Exists(c => c.Id == card.Id)) ||
                            (user.CardStack.Count(c => c.Id == card.Id) < cards.Count(c => c.Id == card.Id)))
                            return 0;
                    }
                    int affectedRows = user.CardDeck.Count;
                    user.CardDeck = new List<ACard>();
                    foreach (ACard card in cards)
                    {
                        user.CardDeck.Add(card);
                        affectedRows += 1;
                    }
                    return affectedRows;
                });
            //act
            int affectedRowsFirstUser = mockRepository.Object.SetCardDeck(mockUsers[0].CardStack, mockUsers[0]);
            int affectedRowsSecondUser = mockRepository.Object.SetCardDeck(mockUsers[0].CardStack, mockUsers[1]);
            //assert
            mockRepository.Verify(mr=>mr.SetCardDeck(mockUsers[0].CardStack, mockUsers[0]));
            mockRepository.Verify(mr=>mr.SetCardDeck(mockUsers[0].CardStack, mockUsers[1]));
            Assert.IsTrue(affectedRowsFirstUser>=4);
            Assert.IsTrue(affectedRowsSecondUser==0);
        }
        [Test]
        public void DeductCoinsMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.CoinsDeductible(It.IsAny<int>(), It.IsAny<User>())).Returns(
                (int coinAmount,User user) => user!=null && coinAmount>=0 && user.Coins>=coinAmount);
            mockRepository.Setup(mr => mr.DeductCoins(It.IsAny<int>(), It.IsAny<User>())).Returns(
                (int coinAmount, User user) =>
                {
                    if (!mockRepository.Object.CoinsDeductible(coinAmount,user))
                        return 0;

                    user.Coins -= coinAmount;
                    
                    return 1;
                });
            //act
            int affectedRowsFirstUser = mockRepository.Object.DeductCoins(4, mockUsers[0]);
            int affectedRowsSecondUser = mockRepository.Object.DeductCoins(10, mockUsers[1]);
            //assert
            mockRepository.Verify(mr=>mr.CoinsDeductible(4,mockUsers[0]));
            mockRepository.Verify(mr=>mr.CoinsDeductible(10,mockUsers[1]));
            mockRepository.Verify(mr=>mr.DeductCoins(4,mockUsers[0]));
            mockRepository.Verify(mr=>mr.DeductCoins(10,mockUsers[1]));
            Assert.AreEqual(1,affectedRowsFirstUser);
            Assert.AreEqual(0,affectedRowsSecondUser);
        }
        [Test]
        public void CoinsDeductibleMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.CoinsDeductible(It.IsAny<int>(), It.IsAny<User>())).Returns(
                (int coinAmount,User user) => user!=null && coinAmount>=0 && user.Coins>=coinAmount);
            //act
            bool coinsDeductibleFirstUser = mockRepository.Object.CoinsDeductible(10, mockUsers[0]);
            bool coinsDeductibleSecondUser = mockRepository.Object.CoinsDeductible(3, mockUsers[1]);
            //assert
            mockRepository.Verify(mr=>mr.CoinsDeductible(10,mockUsers[0]));
            mockRepository.Verify(mr=>mr.CoinsDeductible(3,mockUsers[1]));
            Assert.IsTrue(coinsDeductibleFirstUser);
            Assert.IsFalse(coinsDeductibleSecondUser);
        }
        
        

        

        

    }
}