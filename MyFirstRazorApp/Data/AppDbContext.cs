using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Data
{
    public class AppDbContext : DbContext
    //public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

        //public DbSet<User> Uers { get; set; }
        //public DbSet<Role> Roles { get; set; } //ENUM

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }

    }
}