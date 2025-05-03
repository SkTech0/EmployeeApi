using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication2.Models;

namespace WebApplication2.DataAcces
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // State has many Districts
            modelBuilder.Entity<State>()
                .HasMany(s => s.Districts)
                .WithOne(d => d.State)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.Cascade);

            // District has many Employees
            modelBuilder.Entity<District>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.District)
                .HasForeignKey(e => e.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
