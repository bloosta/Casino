using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CasinoConsoleApp
{
    public class CasinoContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=your_database;Username=your_username;Password=your_password");
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=casino; Username = postgres; Password = postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientSession>()
                .HasKey(cs => new { cs.ClientId, cs.SessionId });

            modelBuilder.Entity<ClientSession>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientSessions)
                .HasForeignKey(cs => cs.ClientId);

            modelBuilder.Entity<ClientSession>()
                .HasOne(cs => cs.Session)
                .WithMany(s => s.ClientSessions)
                .HasForeignKey(cs => cs.SessionId);
        }
    }
}
