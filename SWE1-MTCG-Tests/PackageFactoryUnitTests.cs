using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SWE1_MTCG;

namespace SWE1_MTCG_Tests
{
    [TestFixture]
    public class PackageFactoryUnitTests
    {

        private IPackageFactory packageFactory;
        [SetUp]
        public void SetUp()
        {
            packageFactory = new StandardPackageFactory();
        }
        
        [Test]
        public void CreatePackageTest()
        {
            //arrange
            List<ICard> standardCards;
            //act
            standardCards = packageFactory.CreatePackage().OpenPackage().ToList();
            //assert
            CollectionAssert.AreEqual(StandardPackageFactory.standardPackage, standardCards);
        }
        [Test]
        public void CreateHighRarityPackageTest()
        {
            //arrange
            List<ICard> highRarityCards;
            //act
            highRarityCards = packageFactory.CreateHighRarityPackage().OpenPackage().ToList();
            //assert
            CollectionAssert.AreEqual(StandardPackageFactory.highRarityPackage, highRarityCards);
        }
    }
}