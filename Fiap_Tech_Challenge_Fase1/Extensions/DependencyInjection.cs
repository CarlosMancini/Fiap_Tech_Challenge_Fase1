using Core.Gateways;
using Core.Repository;
using Core.Services;
using Fiap_Tech_Challenge_Fase1.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;

namespace Fiap_Tech_Challenge_Fase1.Extensions
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


            //services
            serviceCollection.AddScoped<IContatoService, ContatoService>();
        }
    }
}
