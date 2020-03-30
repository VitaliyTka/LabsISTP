using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lab1_ISTP
{
    public partial class DBPerfumerContext : DbContext
    {
        public DBPerfumerContext()
        {
        }

        public DBPerfumerContext(DbContextOptions<DBPerfumerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassificationsPerfumes> ClassificationsPerfumes { get; set; }
        public virtual DbSet<Currencys> Currencys { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<Packings> Packings { get; set; }
        public virtual DbSet<Perfumes> Perfumes { get; set; }
        public virtual DbSet<PerfumesInformations> PerfumesInformations { get; set; }
        public virtual DbSet<Prices> Prices { get; set; }
        public virtual DbSet<TypesPackings> TypesPackings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MOGI2N1\\SQLEXPRESS; Database=DBPerfumer; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassificationsPerfumes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassificationPerfume)
                    .HasColumnName("classificationPerfume")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Currencys>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Manufacturers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Manufacturer).HasMaxLength(50);
            });

            modelBuilder.Entity<Packings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PerfumeInformationId).HasColumnName("perfumeInformationID");

                entity.Property(e => e.TypePackingId).HasColumnName("typePackingID");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.PerfumeInformation)
                    .WithMany(p => p.Packings)
                    .HasForeignKey(d => d.PerfumeInformationId)
                    .HasConstraintName("FK_Packings_PerfumesInformations");

                entity.HasOne(d => d.TypePacking)
                    .WithMany(p => p.Packings)
                    .HasForeignKey(d => d.TypePackingId)
                    .HasConstraintName("FK_Packings_TypesPackings");
            });

            modelBuilder.Entity<Perfumes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PackingId).HasColumnName("packingID");

                entity.Property(e => e.PriceId).HasColumnName("priceID");

                entity.HasOne(d => d.Packing)
                    .WithMany(p => p.Perfumes)
                    .HasForeignKey(d => d.PackingId)
                    .HasConstraintName("FK_Perfumes_Packings");

                entity.HasOne(d => d.Price)
                    .WithMany(p => p.Perfumes)
                    .HasForeignKey(d => d.PriceId)
                    .HasConstraintName("FK_Perfumes_Prices");
            });

            modelBuilder.Entity<PerfumesInformations>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassificationPerfumeId).HasColumnName("classificationPerfumeID");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturerID");

                entity.Property(e => e.PerfumeName)
                    .HasColumnName("perfumeName")
                    .HasMaxLength(50);

                entity.Property(e => e.PicturePath)
                    .HasColumnName("picturePath")
                    .HasMaxLength(50);

                entity.Property(e => e.YearOfIssue).HasColumnName("yearOfIssue");

                entity.HasOne(d => d.ClassificationPerfume)
                    .WithMany(p => p.PerfumesInformations)
                    .HasForeignKey(d => d.ClassificationPerfumeId)
                    .HasConstraintName("FK_PerfumesInformations_ClassificationsPerfumes1");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.PerfumesInformations)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_PerfumesInformations_Manufacturers1");
            });

            modelBuilder.Entity<Prices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CurrencyId).HasColumnName("currencyID");

                entity.Property(e => e.DateCreation)
                    .HasColumnName("dateCreation")
                    .HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Prices_Currencys");
            });

            modelBuilder.Entity<TypesPackings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TypePacking)
                    .HasColumnName("typePacking")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
