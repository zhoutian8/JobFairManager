using JobFairManager.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFairManager.Data
{
    public class JobFairDbContext : DbContext
    {
        public JobFairDbContext(DbContextOptions<JobFairDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Resumes> Resumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
