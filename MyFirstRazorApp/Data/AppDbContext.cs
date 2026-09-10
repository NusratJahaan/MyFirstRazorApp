using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Bangla", Description = "Bengali Language" },
                new Course { Id = 2, Name = "English", Description = "English Language" },
                new Course { Id = 3, Name = "Math", Description = "Mathematics" },
                new Course { Id = 4, Name = "Science", Description = "General Science" },
                new Course { Id = 5, Name = "History", Description = "World History" },
                new Course { Id = 6, Name = "Geography", Description = "Geography" },
                new Course { Id = 7, Name = "Physics", Description = "Physics" },
                new Course { Id = 8, Name = "Chemistry", Description = "Chemistry" }
            );
        }
    }
}