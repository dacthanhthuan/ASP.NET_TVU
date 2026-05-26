using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace library_management_system.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<USER> Users { get; set; }
    }
}