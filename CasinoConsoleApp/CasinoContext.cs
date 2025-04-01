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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sessions)
                .WithMany(s => s.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientSession",
                    j => j.HasOne<Session>().WithMany().HasForeignKey("SessionId"),
                    j => j.HasOne<Client>().WithMany().HasForeignKey("ClientId"));
        }
    }
}
