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
            // Конфигурация таблицы ClientGames
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Players)
                .WithMany(c => c.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientGames",  // имя таблицы
                    j => j
                        .HasOne<Client>()
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Game>()
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade))
                // явный составной ключ
                .HasKey("ClientId", "GameId");
        }

    }
}
