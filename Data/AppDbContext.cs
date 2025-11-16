using Microsoft.EntityFrameworkCore;
using CodeBidder.Models;

namespace CodeBidder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProjectDetailsModel> ProjectDetails { get; set; }
        public DbSet<UserProject> UserProject { get; set; }
        public DbSet<Quotation> Quotations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.ProjectDetail)
                .WithMany()
                .HasForeignKey(up => up.ProjectDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
