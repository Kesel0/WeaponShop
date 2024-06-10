using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WeaponShop.Models;

public partial class WeaponshopContext : DbContext
{
    public WeaponshopContext()
    {
    }

    public WeaponshopContext(DbContextOptions<WeaponshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=weaponshop;Username=postgres;Password=2461382;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("prodtype", new[] { "Weapon", "Accessory", "Ammo" });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('productid'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Caliber)
                .HasMaxLength(100)
                .HasColumnName("caliber");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("character varying")
                .HasColumnName("product_description");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice).HasColumnName("product_price");
            entity.Property(e => e.ProductSubtype)
                .HasColumnType("character varying")
                .HasColumnName("product_subtype");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('userid'::regclass)")
                .HasColumnName("Id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("Email");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("Username");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("Password");
        });
        modelBuilder.HasSequence("productid").StartsAt(1000L);
        modelBuilder.HasSequence("userid");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
