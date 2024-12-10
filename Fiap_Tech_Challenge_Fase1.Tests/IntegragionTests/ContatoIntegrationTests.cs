using Core.Entities;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
            var novoContato = new Contato
            {
                ContatoNome = "Teste Contato",
                ContatoEmail = "teste@contato.com",
                ContatoTelefone = "123456789",
                RegiaoId = 1,
                DataCriacao = DateTime.Now
            };

            // Configurando o banco de dados em memória
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
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
            // Configurando o banco de dados em memória
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                // Limpar e semear dados no banco de dados em memória
                dbContext.Contatos.RemoveRange(dbContext.Contatos);
                await dbContext.SaveChangesAsync();

                var contatoExistente = new Contato
                {
                    ContatoNome = "Teste Contato",
                    ContatoEmail = "teste@contato.com",
                    ContatoTelefone = "123456789",
                    RegiaoId = 1
                };

                await dbContext.Contatos.AddAsync(contatoExistente);
                await dbContext.SaveChangesAsync();

                var contatoDuplicado = new Contato
                {
                    ContatoNome = "Teste Contato",
                    ContatoEmail = "duplicado@contato.com",
                    ContatoTelefone = "123456789", // Mesmo telefone do contato existente
                    RegiaoId = 1
                };

                // Act
                var response = await _client.PostAsJsonAsync("/contacts", contatoDuplicado);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}