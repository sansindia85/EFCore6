using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    //DbContext : Provides logic for EF Core to interact with your database
    public class SamuraiContext : DbContext
    {
        //ASP Net core app is able to pass options to the constructor from dependency injection.
        public SamuraiContext(DbContextOptions<SamuraiContext> options)
            : base(options)
        {

        }

        public DbSet<Samurai> Samurais { get; set; } = null!;
        public DbSet<Quote> Quotes { get; set; } = null!;
        public DbSet<Battle>? Battles { get; set; }
        
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