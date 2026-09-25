using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Models.StudentModels;

namespace MyFirstRazorApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ============ Existing Business Tables ============
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        // ============ Court Case Tables ============
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<OffenceLookUp> OffenceLookUps { get; set; }
        public DbSet<Offence> Offences { get; set; }
        public DbSet<Witness> Witnesses { get; set; }
        public DbSet<Warrant> Warrants { get; set; }
        public DbSet<ReturnOfService> ReturnsOfService { get; set; }
        public DbSet<Judgement> Judgements { get; set; }
        public DbSet<CaseHistory> CaseHistories { get; set; }

        // ✅ Seed Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OffenceLookUp>().HasData(
                new OffenceLookUp { Id = 1, Code = "14-40.1", Description = "Domestic Violence" },
                new OffenceLookUp { Id = 2, Code = "20-141", Description = "Speed Offence" },
                new OffenceLookUp { Id = 3, Code = "90-95", Description = "Traffic Violation" },
                new OffenceLookUp { Id = 4, Code = "14-33", Description = "Assault" },
                new OffenceLookUp { Id = 5, Code = "14-72", Description = "Theft" },
                new OffenceLookUp { Id = 6, Code = "14-100", Description = "Fraud" },
                new OffenceLookUp { Id = 7, Code = "14-196", Description = "Trespassing" },
                new OffenceLookUp { Id = 8, Code = "14-277", Description = "Disorderly Conduct" }
            );
        }
    }
}