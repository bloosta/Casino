using Microsoft.EntityFrameworkCore;
using CasinoConsoleApp.Core.Entities;

namespace CasinoConsoleApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Players)
                .WithMany(c => c.Games)
                .UsingEntity(j => j.ToTable("ClientGames"));
        }
    }
}
