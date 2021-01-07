using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SWE1_MTCG;

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
                 Username = "kretmatt"
             },
             new User
             {
                 Id=2,
                 Bio = "Test Bio 2 ",
                 Username = "testuser"
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




    }
}