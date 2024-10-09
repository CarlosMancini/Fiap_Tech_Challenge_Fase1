using Core.Entities;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;
    protected readonly ApplicationDbContext _dbContext;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        // Criar cliente HTTP
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Substituir DbContext pelo InMemoryDb para testes
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        }).CreateClient();

        // Obter instância do DbContext
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            _dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
        }
    }

    protected void SeedData()
    {
        // Método para inserir dados de exemplo no banco em memória, se necessário
        _dbContext.Contatos.Add(new Contato
        {
            ContatoNome = "Contato Existente",
            ContatoEmail = "duplicado@contato.com",
            ContatoTelefone = "123456789",
            RegiaoId = 1 // Ajuste conforme necessário
        });
        _dbContext.SaveChanges();
    }
}
