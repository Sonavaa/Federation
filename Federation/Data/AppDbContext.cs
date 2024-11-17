using FederationTask.Models.Base;
using FederationTask.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace FederationTask.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<club> Clubs { get; set; }
        public DbSet<team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<club>()
          .HasMany(c => c.Teams)
          .WithOne(t => t.Club)
          .HasForeignKey(t => t.ClubId);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<baseAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "User";
                        entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        break;

                    case EntityState.Modified:

                        entry.Entity.UpdatedBy = "User";
                        entry.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
