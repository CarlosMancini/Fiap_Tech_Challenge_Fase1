using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

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
            ContatoNome = "Teste Contato",
            ContatoEmail = "teste@contato.com",
            ContatoTelefone = "123456789",
            RegiaoId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contatos", novoContato);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var contatoCadastrado = await _dbContext.Contatos.FirstOrDefaultAsync(c => c.ContatoEmail == "teste@contato.com");
        Assert.NotNull(contatoCadastrado);
    }

    [Fact]
    public async Task Cadastrar_DeveRetornarBadRequest_QuandoEmailDuplicado()
    {
        // Arrange
        SeedData(); // Insere um contato com o mesmo e-mail no banco de dados

        var contatoDuplicado = new
        {
            ContatoNome = "Teste Contato",
            ContatoEmail = "duplicado@contato.com",
            ContatoTelefone = "123456789",
            RegiaoId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contatos", contatoDuplicado);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
