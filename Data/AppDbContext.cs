using Microsoft.EntityFrameworkCore;
using CodeBidder.Models;

namespace CodeBidder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Existing DbSet for User
        public DbSet<User> Users { get; set; }

        // New DbSet for ProjectDetailsModel
        public DbSet<ProjectDetailsModel> ProjectDetails { get; set; }

        public DbSet<UserProject> UserProject { get; set; }







    }
}
