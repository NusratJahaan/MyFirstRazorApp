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
    }
}