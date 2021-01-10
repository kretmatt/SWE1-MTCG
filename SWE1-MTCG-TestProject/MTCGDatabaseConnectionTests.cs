using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SWE1_MTCG;
using SWE1_MTCG.DBFeature;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class MTCGDatabaseConnectionTests
    {
        private Mock<INpgsqlCommand> mockStatement;
        private Mock<INpgsqlDataReader> mockReader;
        private List<object[]> mockCollection;
        private int expectedResult;
        
        [SetUp]
        public void SetUp()
        {
            mockStatement = new Mock<INpgsqlCommand>();
            mockReader= new Mock<INpgsqlDataReader>();
            mockCollection=new List<object[]>();
            expectedResult = 1;
        }
        
        [Test]
        public void QueryDatabaseMock_ExpectedCallsWereMadeReturnsMockCollection()
        {
            //arrange
            mockStatement.Setup(ms => ms.ExecuteReader()).Returns(mockReader.Object);
            MTCGDatabaseConnection mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            mockReader.Setup(mr => mr.Read()).Returns(false);
            //act
            List<object[]> result = mtcgDatabaseConnection.QueryDatabase(mockStatement.Object);
            //assert - Connection must be set and ExecuteReader must be called once
            mockStatement.VerifySet(ms=>ms.Connection);
            mockStatement.Verify(ms=>ms.ExecuteReader(), Times.Once);
            mockReader.Verify(mr=>mr.Read(), Times.Once);
            //Due to mockReader.Read() returning false, a while loop is not executed. --> mr.FieldCount() is never called
            mockReader.Verify(mr=>mr.FieldCount(),Times.Never);
            CollectionAssert.AreEqual(mockCollection, result);
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