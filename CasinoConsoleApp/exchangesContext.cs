using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class exchangesContext : DbContext
    {
        public exchangesContext()
        {
        }

        public exchangesContext(DbContextOptions<exchangesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Deals> Deals { get; set; }
        public virtual DbSet<Issuer> Issuer { get; set; }
        public virtual DbSet<NonUpdatableView> NonUpdatableView { get; set; }
        public virtual DbSet<Shares> Shares { get; set; }
        public virtual DbSet<Shops> Shops { get; set; }
        public virtual DbSet<StockRecords> StockRecords { get; set; }
        public virtual DbSet<TestTableSuperuser> TestTableSuperuser { get; set; }
        public virtual DbSet<UpdatableView> UpdatableView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=exchanges;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("tablefunc");

            modelBuilder.Entity<Deals>(entity =>
            {
                entity.ToTable("deals");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Retail).HasColumnName("retail");

                entity.Property(e => e.ShareId).HasColumnName("share_id");

                entity.Property(e => e.ShopId).HasColumnName("shop_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.HasOne(d => d.Share)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.ShareId)
                    .HasConstraintName("deals_share_id_fkey");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("deals_shop_id_fkey");
            });

            modelBuilder.Entity<Issuer>(entity =>
            {
                entity.ToTable("issuer");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Share).HasColumnName("share");
            });

            modelBuilder.Entity<NonUpdatableView>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("non_updatable_view");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.DealId).HasColumnName("deal_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Retail).HasColumnName("retail");

                entity.Property(e => e.ShareName).HasColumnName("share_name");

                entity.Property(e => e.ShopName).HasColumnName("shop_name");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Shares>(entity =>
            {
                entity.ToTable("shares");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.IssuerId).HasColumnName("issuer_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Nominal).HasColumnName("nominal");

                entity.HasOne(d => d.Issuer)
                    .WithMany(p => p.Shares)
                    .HasForeignKey(d => d.IssuerId)
                    .HasConstraintName("shares_issuer_id_fkey");
            });

            modelBuilder.Entity<Shops>(entity =>
            {
                entity.ToTable("shops");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<StockRecords>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("stock_records");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("purchase_date")
                    .HasColumnType("date");

                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<TestTableSuperuser>(entity =>
            {
                entity.ToTable("test_table_superuser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<UpdatableView>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("updatable_view");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Retail).HasColumnName("retail");

                entity.Property(e => e.ShareId).HasColumnName("share_id");

                entity.Property(e => e.ShopId).HasColumnName("shop_id");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
