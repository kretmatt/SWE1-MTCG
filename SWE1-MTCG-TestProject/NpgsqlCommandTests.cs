using Moq;
using Npgsql;
using NUnit.Framework;
using SWE1_MTCG;
using SWE1_MTCG.DBFeature;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class NpgsqlCommandTests
    {
        private INpgsqlDataReader _npgsqlDataReader;
        private INpgsqlCommand _npgsqlCommand;
        private int expectedValue;
        
        [SetUp]
        public void SetUp()
        {
            _npgsqlCommand = new NpsqlCommand("Select * From Mock;");
            _npgsqlDataReader = _npgsqlCommand.ExecuteReader();
            expectedValue = 0;
        }
        
        [Test]
        public void ExecuteNonQueryMock_NoConnectionSetReturns0()
        {
            //arrange
            var executeNonQueryMock = new Mock<INpgsqlCommand>();
            executeNonQueryMock.Setup(enq => enq.ExecuteNonQuery()).Returns(expectedValue);
            int result;
            //act
            result = executeNonQueryMock.Object.ExecuteNonQuery();
            //assert
            executeNonQueryMock.Verify(enq=>enq.ExecuteNonQuery());
            Assert.AreEqual(expectedValue,result);
        }

        [Test]
        public void ExecuteReaderMock_NoConnectionSetReturnsNull()
        {
            //arrange
            var executeReaderMock = new Mock<INpgsqlCommand>();
            executeReaderMock.Setup(erm => erm.ExecuteReader()).Returns(_npgsqlDataReader);
            //act
            INpgsqlDataReader result = executeReaderMock.Object.ExecuteReader();
            //assert
            executeReaderMock.Verify(erm=>erm.ExecuteReader());
            Assert.IsNull(result);
        }
    }
}