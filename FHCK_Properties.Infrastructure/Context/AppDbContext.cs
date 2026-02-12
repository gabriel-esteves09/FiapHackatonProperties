using FHCK_Properties.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FHCK_Properties.Infrastructure.Context
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Property> Property { get; set; }
        public DbSet<Plot> Plot { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
