using Moq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class SessionRepositoryTests
    {
        private Mock<ISessionRepository> mockRepository;
        private string mockUsername;
        private string mockRandomUser;
        private string mockCorrectPassword;
        private string mockIncorrectPassword;
        
        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<ISessionRepository>();
            mockCorrectPassword = "123456";
            mockUsername = "kretmatt";
            mockIncorrectPassword = "TEStMOCK";
            mockRandomUser = "random";
        }
        [Test]
        public void LoginMock_LoginSuccessAndLoginFail()
        {
            //arrange - return true if correct username+password combination is passed, else return false
            mockRepository.Setup(mr => mr.Login(mockUsername, mockCorrectPassword)).Returns(true);
            mockRepository.Setup(mr => mr.Login(It.IsAny<string>(), It.Is<string>(s => s != mockCorrectPassword))).Returns(false);
            //act           
            bool mockUsernameLogin = mockRepository.Object.Login(mockUsername, mockCorrectPassword);
            bool mockUsernameFalsePasswordLogin = mockRepository.Object.Login(mockUsername, mockIncorrectPassword);
            //assert
            mockRepository.Verify(mr=>mr.Login(mockUsername,mockCorrectPassword));
            mockRepository.Verify(mr=>mr.Login(mockUsername, mockIncorrectPassword));
            Assert.IsFalse(mockUsernameFalsePasswordLogin);
            Assert.IsTrue(mockUsernameLogin);
        }
        [Test]
        public void CheckIfInValidSessionMock()
        {
            //arrange - return true if mockUsername is passed, else return false
            mockRepository.Setup(mr => mr.CheckIfInValidSession(mockUsername)).Returns(true);
            mockRepository.Setup(mr => mr.CheckIfInValidSession(It.Is<string>(s => s != mockUsername))).Returns(false);
            //act
            bool mockUsernameInSession = mockRepository.Object.CheckIfInValidSession(mockUsername);
            bool mockRandomUserInSession = mockRepository.Object.CheckIfInValidSession(mockRandomUser);
            //assert
            mockRepository.Verify(mr=>mr.CheckIfInValidSession(mockUsername));
            mockRepository.Verify(mr=>mr.CheckIfInValidSession(mockRandomUser));
            Assert.IsTrue(mockUsernameInSession);
            Assert.IsFalse(mockRandomUserInSession);
        }
        [Test]
        public void LogoutMock()
        {
            //arrange
            mockRepository.Setup(mr => mr.Logout(mockUsername)).Returns(true);
            mockRepository.Setup(mr => mr.Logout(It.Is<string>(s => s != mockUsername))).Returns(false);
            //act
            bool mockUsernameLogout = mockRepository.Object.Logout(mockUsername);
            bool mockRandomUserLogout = mockRepository.Object.Logout(mockRandomUser);
            //assert
            mockRepository.Verify(mr=>mr.Logout(mockUsername));
            mockRepository.Verify(mr=>mr.Logout(mockRandomUser));
            Assert.IsTrue(mockUsernameLogout);
            Assert.IsFalse(mockRandomUserLogout);
        }
        
    }
}