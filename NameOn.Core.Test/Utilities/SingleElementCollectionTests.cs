using NameOn.Core.Utilities;
using NUnit.Framework;
using System.Linq;

namespace NameOn.Core.Test.Utilities
{
    public class SingleElementCollectionTests
    {
        [Test]
        public void Test()
        {
            var collection = new SingleElementCollection<int>(2);
            Assert.AreEqual(new[] { 2 }, collection.ToArray());
        }
    }
}