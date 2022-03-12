using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SamuraiApp.Data;
using System.Linq;

namespace Tests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<SamuraiAPI.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<SamuraiContext>));

                services.Remove(descriptor);

                services.AddDbContext<SamuraiContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SamuraiContext>();
                    
                    db.Database.EnsureCreated();                    
                }
            });
        }
    }
}
