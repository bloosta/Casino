using Microsoft.EntityFrameworkCore;
namespace CasinoConsoleApp
{
    public class CasinoContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ClientSession> ClientSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=casino; Username = postgres; Password = postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientSession>()
                .HasKey(cs => new { cs.ClientId, cs.SessionId });

            modelBuilder.Entity<ClientSession>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientSessions)
                .HasForeignKey(cs => cs.ClientId)
                .HasConstraintName("FK_ClientSession_Client");

            modelBuilder.Entity<ClientSession>()
                .HasOne(cs => cs.Session)
                .WithMany(s => s.ClientSessions)
                .HasForeignKey(cs => cs.SessionId)
                .HasConstraintName("FK_ClientSession_Session");
        }

    }
}
