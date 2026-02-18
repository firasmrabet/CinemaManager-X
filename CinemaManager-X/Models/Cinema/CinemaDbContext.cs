using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager_X.Models.Cinema;

public partial class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CinemaDB;Trusted_Connection=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Movie");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("Title");
            entity.Property(e => e.Genre)
                .HasMaxLength(20)
                .HasColumnName("Genre");
            entity.Property(e => e.ProducerId).HasColumnName("ProducerId");

            entity.HasOne(d => d.Producer).WithMany(p => p.Movies)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Movie_Prod");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Producer");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("Name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(30)
                .HasColumnName("Nationality");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("Email");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
