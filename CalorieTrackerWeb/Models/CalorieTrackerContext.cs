using Microsoft.EntityFrameworkCore;
using CalorieTrackerWeb.Models;

namespace CalorieTrackerWeb.Models
{
    public class CalorieTrackerContext : DbContext
    {
        public CalorieTrackerContext(DbContextOptions<CalorieTrackerContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder != null)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CalorieTrackerDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<FoodEntry> FoodEntries { get; set; }
        public DbSet<Workout> Workouts { get; set; }

    }
}
