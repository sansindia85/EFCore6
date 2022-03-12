using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Linq;

namespace SamuraiAPI.Test
{
    [TestClass]
    public class BusinessDataLogicTests
    {
        [TestMethod]
        public void CanAddSamuraisByName()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanAddSamuraisByName");

            using var context = new SamuraiContext(builder.Options);
            var businessLogic = new BusinessDataLogic(context);

            var nameList = new string[] { "Kikuchiyo", "Kyuzo", "Rikchi" };

            var result = businessLogic.AddSamurasiByName(nameList);
            Assert.AreEqual(nameList.Length, result);
        }

        [TestMethod]
        public void CanInsertSingleSamurai()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSingleSamurai");

            using (var context = new SamuraiContext(builder.Options))
            {
                var businessLogic = new BusinessDataLogic(context);
                businessLogic.InsertNewSamurai(new Samurai());
            }

            using (var context2 = new SamuraiContext(builder.Options))
            {
                Assert.AreEqual(1, context2.Samurais.Count());
            }
        }
    }
}
