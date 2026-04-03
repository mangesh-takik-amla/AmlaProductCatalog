using Microsoft.EntityFrameworkCore;
using AmlaProductCatalog.Models;

namespace AmlaProductCatalog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }
    }
}