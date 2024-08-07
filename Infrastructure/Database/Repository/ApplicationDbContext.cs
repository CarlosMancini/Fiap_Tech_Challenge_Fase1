using Infrastructure.Database.Repository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoConfiguration());
            modelBuilder.ApplyConfiguration(new RegiaoConfiguration());
        }
    }
}
