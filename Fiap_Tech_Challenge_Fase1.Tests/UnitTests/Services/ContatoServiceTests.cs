using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Services;
using Fiap_Tech_Challenge_Fase1.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fiap_Tech_Challenge_Fase1.Tests.UnitTests.Services
{
    public class ContatoServiceTests : IDisposable
    {
        private readonly IContatoService _contatoService;
        private readonly ApplicationDbContext _context;

        public ContatoServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            var contatoRepository = new ContatoRepository(_context);
            var regiaoRepository = new RegiaoRepository(_context);
            var brasilGateway = new BrasilGateway();

            _contatoService = new ContatoService(contatoRepository, regiaoRepository, brasilGateway);
        }

        [Fact]
        public async Task Cadastrar_DeveLancarException_QuandoContatoJaExiste()
        {
            // Arrange
            var contatoExistente = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            await _contatoService.Cadastrar(contatoExistente);

            var novoContato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _contatoService.Cadastrar(novoContato));
            Assert.Equal("Ja existe um contato com o mesmo nome e telefone.", exception.Message);
        }

        [Fact]
        public async Task Cadastrar_DeveChamarRepository_QuandoContatoEhValido()
        {
            // Arrange
            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            Assert.Contains(_context.Contatos, c => c.ContatoNome == contato.ContatoNome);
        }

        [Fact]
        public async Task Cadastrar_DeveChamarCadastrarRegiao_QuandoRegiaoEhNova()
        {
            // Arrange
            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "1123456789",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            string regiaoNome = "S�o Paulo";

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            Assert.NotNull(await _context.Regioes.FirstOrDefaultAsync(r => r.RegiaoNome == regiaoNome));
            Assert.Equal(1, contato.RegiaoId); // Verifica se o ID da nova regi�o foi atribu�do corretamente ao contato
        }

        [Fact]
        public async Task Alterar_DeveLancarException_QuandoContatoJaExiste()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };
            var existingContato = new Contato
            {
                Id = 2,
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            await _context.Contatos.AddAsync(existingContato);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _contatoService.Alterar(contato));
        }

        [Fact]
        public async Task Alterar_DeveChamarRepository_QuandoContatoEhValido()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "1198563254",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            var regiao = new Regiao { Id = 1, RegiaoNome = "S�o Paulo", RegiaoDdd = 11 };
            await _context.Regioes.AddAsync(regiao);
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();

            // Altera telefone do contato
            contato.ContatoTelefone = "1132336598";

            // Act
            await _contatoService.Alterar(contato);

            // Assert
            var contatoAlterado = await _context.Contatos.FindAsync(contato.Id);
            Assert.Equal(contato.ContatoNome, contatoAlterado.ContatoNome);
        }

        // M�todo Dispose para limpar o banco de dados em mem�ria ap�s cada teste
        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Limpa o banco de dados
            _context.Dispose();
        }
    }
}
