using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TimeTracker.API.Data
{
    public class DataContext : IdentityDbContext<User> // DbContext (identity tables are added by inheriting from IdentityDbContext)
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        // always include nested objects instead of manually adding .Include()
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeEntry>().Navigation(c => c.Project).AutoInclude();
            modelBuilder.Entity<Project>().Navigation(c => c.ProjectDetails).AutoInclude();
            base.OnModelCreating(modelBuilder); // identity methods that configure the identity tables
        }

        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }
        // public DbSet<User> Users { get; set; } // access via identity instead
    }
}
