using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class PackageUnitTests
    {
        [Test]
        public void AddCardToPackageMock()
        {
            //arrange
            var addCardMock = new Mock<IPackage>();
            //act
            addCardMock.Object.AddCardToPackage(StandardPackageFactory.standardPackage[0]);
            //assert
            addCardMock.Verify(addCard => addCard.AddCardToPackage(StandardPackageFactory.standardPackage[0]));
        }

        [Test]
        public void AddCardRangeMock()
        {
            //arrange
            var addCardRangeMock = new Mock<IPackage>();
            //act
            addCardRangeMock.Object.AddCardRange(StandardPackageFactory.standardPackage);
            //assert
            addCardRangeMock.Verify(addCard => addCard.AddCardRange(StandardPackageFactory.standardPackage));
        }

        [Test]
        public void OpenPackageTest()
        {
            //arrange 
            IPackage package = new StandardPackage();
            List<ICard> cards = new List<ICard>();
            //act
            package.AddCardRange(StandardPackageFactory.standardPackage);
            cards = package.OpenPackage().ToList();
            //assert
            CollectionAssert.AreEqual(StandardPackageFactory.standardPackage, cards);
        }
    }
}