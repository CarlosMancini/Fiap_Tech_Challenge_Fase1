using Core.Entities;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Fiap_Tech_Challenge_Fase1.Tests
{
    public class ContatoIntegrationTests : IntegrationTestBase
    {
        public ContatoIntegrationTests(WebApplicationFactory<Program> factory)
            : base(factory) // Passando a fábrica para a classe base
        {
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarOk_QuandoDadosValidos()
        {
            // Arrange
            var novoContato = new Contato
            {
                ContatoNome = "Teste Contato",
                ContatoEmail = "teste@contato.com",
                ContatoTelefone = "123456789",
                RegiaoId = 1,
                DataCriacao = DateTime.Now
            };

            // Act
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

                // Limpar o banco de dados antes do teste
                dbContext.Contatos.RemoveRange(dbContext.Contatos);
                await dbContext.SaveChangesAsync();

                // Adicionar o novo contato
                await dbContext.Contatos.AddAsync(novoContato);
                await dbContext.SaveChangesAsync();

                // Assert
                var contatoCadastrado = await dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "teste@contato.com");
                Assert.NotNull(contatoCadastrado);
                Assert.Equal("Teste Contato", contatoCadastrado.ContatoNome);
            }
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarBadRequest_QuandoNomeETelefoneDuplicado()
        {
            // Arrange
            SeedData();

            var contatoDuplicado = new Contato
            {
                ContatoNome = "Teste Contato",
                ContatoEmail = "duplicado@contato.com",  // Mesmo email e telefone do contato existente
                ContatoTelefone = "123456789",
                RegiaoId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/contacts", contatoDuplicado);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}