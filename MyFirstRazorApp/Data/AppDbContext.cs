using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Models;
using Telerik.SvgIcons;

namespace MyFirstRazorApp.Data
{
    public class AppDbContext : DbContext  // plain DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
    }
}