using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace Fiap_Tech_Challenge_Fase1.Tests
{
    public class ContatoIntegrationTests : IntegrationTestBase
    {
        public ContatoIntegrationTests(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarOk_QuandoDadosValidos()
        {
            // Arrange
            var novoContato = new
            {
                ContatoNome = "Contato de teste",
                ContatoEmail = "user@example.com",
                ContatoTelefone = "61965835428",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/contacts", novoContato);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Acessar o banco de dados dentro de um escopo
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

                var contatoCadastrado = await dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "user@example.com");
                Assert.NotNull(contatoCadastrado);
            }
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarBadRequest_QuandoNomeETelefoneDuplicado()
        {
            // Arrange
            SeedData();

            var contatoDuplicado = new
            {
                ContatoNome = "Teste Contato",
                ContatoEmail = "duplicado@contato.com",
                ContatoTelefone = "123456789",
                RegiaoId = 1,
            };

            // Act
            var response = await _client.PostAsJsonAsync("/contacts", contatoDuplicado);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
