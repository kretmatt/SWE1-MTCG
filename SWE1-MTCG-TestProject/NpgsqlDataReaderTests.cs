using NUnit.Framework;
using SWE1_MTCG;
using Moq;
using SWE1_MTCG.DBFeature;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class NpgsqlDataReaderTests
    {
        private bool readResult;
        private string getValueResult;
        private int valuePos;
        
        [SetUp]
        public void SetUp()
        {
            readResult = true;
            getValueResult = "Mocking";
            valuePos = 0;
        }
        
        [Test]
        public void ReadMock()
        {
            //arrange
            var readMock = new Mock<INpgsqlDataReader>();
            readMock.Setup(rm => rm.Read()).Returns(readResult);
            bool result;
            //act
            result = readMock.Object.Read();
            //assert
            readMock.Verify(rm=>rm.Read());
            Assert.IsTrue(result);
        }

        [Test]
        public void GetValueMock()
        {
            //arrange
            var getValueMock = new Mock<INpgsqlDataReader>();
            getValueMock.Setup(gvm => gvm.GetValue(valuePos)).Returns(getValueResult);
            string result;
            //act
            result = getValueMock.Object.GetValue(valuePos).ToString();
            //assert
            getValueMock.Verify(gvm=>gvm.GetValue(valuePos));
            Assert.AreEqual(getValueResult,result);
        }
    }
}