using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiAPI.Test
{
    [TestClass]
    public class InMemoryTests
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
            //The database name can be shared across methods. It can be reused.
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamurai");


            using (var context = new SamuraiContext(builder.Options))
            {             
                //No need to set properties on new Samurai.
                var samurai = new Samurai();
                context.Samurais.Add(samurai);                
                context.SaveChanges();
                Assert.AreEqual(EntityState.Added, context.Entry(samurai).State);

            }
        }
    }
}
