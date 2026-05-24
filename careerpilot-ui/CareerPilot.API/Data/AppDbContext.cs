using Microsoft.EntityFrameworkCore;
using CareerPilot.API.Models;

namespace CareerPilot.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> 
            options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeAnalysis> ResumeAnalyses { get; set; }
    }
}