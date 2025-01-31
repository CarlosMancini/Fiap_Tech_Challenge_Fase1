using Core.Entities;
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

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Regiao> Regioes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoConfiguration());
            modelBuilder.ApplyConfiguration(new RegiaoConfiguration());
        }
    }
}
