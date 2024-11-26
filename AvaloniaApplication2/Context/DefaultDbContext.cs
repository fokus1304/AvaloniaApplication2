using System;
using System.Collections.Generic;
using AvaloniaApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication2.Context;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext()
    {
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doptov> Doptovs { get; set; }

    public virtual DbSet<ListDoptov> ListDoptovs { get; set; }

    public virtual DbSet<Manufactured> Manufactureds { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=91.186.197.80:5432;Database=default_db;Username=gen_user;password=6?,65\\fIp7(lqq");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doptov>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("doptov_pk");

            entity.ToTable("doptov", "public1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Manufactured).HasColumnName("manufactured");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<ListDoptov>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_doptov_pk");

            entity.ToTable("list_doptov", "public1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdTov).HasColumnName("id_tov");
            entity.Property(e => e.Iddoptow).HasColumnName("iddoptow");

            entity.HasOne(d => d.IdTovNavigation).WithMany(p => p.ListDoptovs)
                .HasForeignKey(d => d.IdTov)
                .HasConstraintName("list_doptov_product_fk");

            entity.HasOne(d => d.IddoptowNavigation).WithMany(p => p.ListDoptovs)
                .HasForeignKey(d => d.Iddoptow)
                .HasConstraintName("list_doptov_doptov_fk");
        });

        modelBuilder.Entity<Manufactured>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufactured_pk");

            entity.ToTable("manufactured", "public1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product", "public1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Doptov).HasColumnName("doptov");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Manufactured).HasColumnName("manufactured");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.DoptovNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Doptov)
                .HasConstraintName("product_list_doptov_fk");

            entity.HasOne(d => d.ManufacturedNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufactured)
                .HasConstraintName("product_manufactured_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
