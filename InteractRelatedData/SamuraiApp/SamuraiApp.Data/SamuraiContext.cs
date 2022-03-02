using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    //DbContext : Provides logic for EF Core to interact with your database
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; } = null!;
        public DbSet<Quote> Quotes { get; set; } = null!;
        public DbSet<Battle>? Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Logging : EF Core way. Not required for ASP.Net Core.
            //Logging : Express the category as an array even if you use only one category.
            optionsBuilder.UseNpgsql("Host=localhost:5435;Database=SamuraiAppData;Username=postgres;Password=sandeep",
                    options => options.MaxBatchSize(100)) //The batch size is not working in NPGSQL
                .LogTo(Console.WriteLine,
                    new[]
                    {
                        DbLoggerCategory.Database.Command.Name,
                        DbLoggerCategory.Database.Transaction.Name
                    },
                    LogLevel.Debug)
                .EnableSensitiveDataLogging(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Database to populate the payload property, DateJoined.
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                    bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("now()");
        }
    }
}