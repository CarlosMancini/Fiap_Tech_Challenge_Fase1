using Core.Entities;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    public readonly HttpClient _client;
    public readonly WebApplicationFactory<Program> _factory;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        // Criar cliente HTTP com banco de dados em memória
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove a configuração do banco de dados existente
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adicionar banco de dados em memória
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        }).CreateClient();
    }

    public void SeedData(ApplicationDbContext dbContext)
    {
        var contatoExistente = new Contato
        {
            ContatoNome = "Teste Contato",
            ContatoEmail = "existente@contato.com",
            ContatoTelefone = "123456789",
            RegiaoId = 1,
            DataCriacao = DateTime.Now
        };

        dbContext.Contatos.Add(contatoExistente);
        dbContext.SaveChanges();
    }
}
