using ConsultaContato.Services;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;

namespace ConsultaContato.Extensions
{
    public static class DependencyInjection
    {
        public static void Inject(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            }, ServiceLifetime.Scoped);

            // Repositories
            serviceCollection.AddScoped<IRegiaoRepository, RegiaoRepository>();
            serviceCollection.AddScoped<IContatoRepository, ContatoRepository>();

            //Gateways
            serviceCollection.AddScoped<IBrasilGateway, BrasilGateway>();

            //Services
            serviceCollection.AddScoped<IContatoService, ContatoService>();
        }
    }
}
