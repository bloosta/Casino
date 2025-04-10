using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class casinoContext : DbContext
    {
        public casinoContext()
        {
        }

        public casinoContext(DbContextOptions<casinoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Clientsession> Clientsession { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=casino;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Clientsession>(entity =>
            {
                entity.HasKey(e => new { e.Clientid, e.Sessionid })
                    .HasName("clientsession_pkey");

                entity.ToTable("clientsession");

                entity.Property(e => e.Clientid).HasColumnName("clientid");

                entity.Property(e => e.Sessionid).HasColumnName("sessionid");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Clientsession)
                    .HasForeignKey(d => d.Clientid)
                    .HasConstraintName("clientsession_clientid_fkey");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Clientsession)
                    .HasForeignKey(d => d.Sessionid)
                    .HasConstraintName("clientsession_sessionid_fkey");
            });

            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.ToTable("sessions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Clients)
                    .IsRequired()
                    .HasColumnName("clients");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Gametype)
                    .IsRequired()
                    .HasColumnName("gametype")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
