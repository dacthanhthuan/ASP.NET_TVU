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

        public DbSet<USER> M_USER { get; set; }
        public DbSet<CATEGORY> M_CATEGORY { get; set; }
        public DbSet<FOOD> M_FOOD { get; set; }
    }
}