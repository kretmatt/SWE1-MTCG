using Moq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class MTCGDatabaseConnectionTests
    {
        private Mock<INpgsqlCommand> mockStatement;
        private Mock<INpgsqlDataReader> mockReader;
        private int expectedResult;
        
        [SetUp]
        public void SetUp()
        {
            mockStatement = new Mock<INpgsqlCommand>();
            mockReader = new Mock<INpgsqlDataReader>();
            expectedResult = 1;
        }
        
        [Test]
        public void QueryDatabaseMock_ExpectedCallsWereMadeReturnsMockReader()
        {
            //arrange
            mockStatement.Setup(ms => ms.ExecuteReader()).Returns(mockReader.Object);
            MTCGDatabaseConnection mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            //act
            INpgsqlDataReader result = mtcgDatabaseConnection.QueryDatabase(mockStatement.Object);
            //assert - Connection must be set and ExecuteReader must be called once
            mockStatement.VerifySet(ms=>ms.Connection);
            mockStatement.Verify(ms=>ms.ExecuteReader(), Times.Once);
            Assert.AreEqual(mockReader.Object, result);
        }
        [Test]
        public void ExecuteStatementMock_ExpectedCallsWereMadeReturnsExpectedResult()
        {
            //arrange
            mockStatement.Setup(ms => ms.ExecuteNonQuery()).Returns(expectedResult);
            MTCGDatabaseConnection mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            //act
            int result = mtcgDatabaseConnection.ExecuteStatement(mockStatement.Object);
            //assert - Connection must be set and ExecuteNonQuery must be called once
            mockStatement.VerifySet(ms=>ms.Connection);
            mockStatement.Verify(ms=>ms.ExecuteNonQuery(), Times.Once);
            Assert.AreEqual(expectedResult, result);
        }
    }
}