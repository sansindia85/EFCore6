using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    //DbContext : Provides logic for EF Core to interact with your database
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; } = null!;
        public DbSet<Quote> Quotes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost:5435;Database=SamuraiAppData;Username=postgres;Password=sandeep");
        }
    }
}