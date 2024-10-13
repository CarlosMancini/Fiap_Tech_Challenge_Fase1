using Core.Entities;
using Core.Interfaces.Services;
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
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();

                var novoContato = new Contato
                {
                    ContatoNome = "Teste Contato",
                    ContatoEmail = "teste@contato.com",
                    ContatoTelefone = "123456789",
                    RegiaoId = 1
                };

                // Act
                await contatoService.Cadastrar(novoContato);

                // Assert
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
                var contatoCadastrado = await dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "teste@contato.com");

                Assert.NotNull(contatoCadastrado);
                Assert.Equal("Teste Contato", contatoCadastrado.ContatoNome);

                // Cleanup
                var contato = await dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "teste@contato.com");
                if (contato != null)
                {
                    dbContext.Contatos.Remove(contato);
                    await dbContext.SaveChangesAsync();
                }
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

            // Cleanup
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

                var contato = await dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "duplicado@contato.com");
                if (contato != null)
                {
                    dbContext.Contatos.Remove(contato);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
