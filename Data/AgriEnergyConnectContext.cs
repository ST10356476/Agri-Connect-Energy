using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Agri_Energy_Connect.Data
{
    public class AgriEnergyConnectContext : DbContext
    {
        public AgriEnergyConnectContext(DbContextOptions<AgriEnergyConnectContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EnergySolutionCategory> EnergySolutionCategories { get; set; }
        public DbSet<EnergySolutionProvider> EnergySolutionProviders { get; set; }
        public DbSet<EnergySolution> EnergySolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Farmer)
                .WithOne(f => f.User)
                .HasForeignKey<Farmer>(f => f.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);

            // Configure indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<ProductCategory>()
                .HasIndex(pc => pc.CategoryName)
                .IsUnique();

            modelBuilder.Entity<EnergySolutionCategory>()
                .HasIndex(ec => ec.CategoryName)
                .IsUnique();

            // Self-referencing relationships
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.ParentCategory)
                .WithMany(pc => pc.Subcategories)
                .HasForeignKey(pc => pc.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Default values
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Role>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Role>()
                .Property(r => r.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Farmer>()
                .Property(f => f.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Farmer>()
                .Property(f => f.IsVerified)
                .HasDefaultValue(false);

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .Property(p => p.IsAvailable)
                .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
                .Property(p => p.CurrencyCode)
                .HasDefaultValue("ZAR");

            modelBuilder.Entity<ProductCategory>()
                .Property(pc => pc.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<EnergySolutionCategory>()
                .Property(ec => ec.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<EnergySolutionProvider>()
                .Property(ep => ep.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EnergySolutionProvider>()
                .Property(ep => ep.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<EnergySolutionProvider>()
                .Property(ep => ep.IsVerified)
                .HasDefaultValue(false);

            modelBuilder.Entity<EnergySolution>()
                .Property(es => es.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EnergySolution>()
                .Property(es => es.IsAvailable)
                .HasDefaultValue(true);

            modelBuilder.Entity<EnergySolution>()
                .Property(es => es.CurrencyCode)
                .HasDefaultValue("ZAR");
        }
    }
}
