using Infrastructure.Repository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoConfiguration());
            modelBuilder.ApplyConfiguration(new RegiaoConfiguration());
        }
    }
}
