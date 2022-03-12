using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SamuraiApp.Domain;

namespace Tests
{
    [TestClass]
    public class WebAPIControllerTest
    {
        private readonly CustomWebApplicationFactory _factory;

        public WebAPIControllerTest()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [TestMethod]
        public async Task GetEndpointReturnsSuccessAndSomeDataFromTheDatabse()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/SamuraisSoc");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObjectList = JsonConvert.DeserializeObject<List<Samurai>>(responseString);
            // Assert
            Assert.AreNotEqual(0, responseObjectList.Count);
        }
    }
}
