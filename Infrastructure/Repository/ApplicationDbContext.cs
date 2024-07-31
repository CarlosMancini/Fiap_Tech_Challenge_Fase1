using Core.Entities;
using Infrastructure.Repository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    internal class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Contato> Contato { get; set; }
        public DbSet<Regiao> Regiao { get; set; }

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
