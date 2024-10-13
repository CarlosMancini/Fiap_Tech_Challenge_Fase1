using Core.Entities;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    public readonly HttpClient _client;
    public readonly WebApplicationFactory<Program> _factory;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        // Criar cliente HTTP
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
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
    }

    public void SeedData()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

            // Inserir dados de exemplo
            dbContext.Contatos.Add(new Contato
            {
                ContatoNome = "Contato Existente",
                ContatoEmail = "duplicado@contato.com",
                ContatoTelefone = "123456789",
                RegiaoId = 1,
                DataCriacao = DateTime.Now,
            });
            dbContext.SaveChanges();
        }
    }
}
