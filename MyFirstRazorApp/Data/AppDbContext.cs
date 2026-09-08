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

        public override int SaveChanges()  //remove
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)  //remove
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields() //remove
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = "System";
                }

                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = "System";
            }
        }
    }
}