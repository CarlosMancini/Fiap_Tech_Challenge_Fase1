using Atualizacao.Services;
using Cadastro.Services;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;

namespace Fiap_Tech_Challenge_Fase1.Tests.UnitTests.Services
{
    public class ContatoServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ICadastroService _cadastroService;
        private readonly IAtualizacaoService _atualizacaoService;


        public ContatoServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            var contatoRepository = new ContatoRepository(_context);
            var regiaoRepository = new RegiaoRepository(_context);
            var brasilGateway = new BrasilGateway();

            _cadastroService = new CadastroService(contatoRepository, regiaoRepository, brasilGateway);
            _atualizacaoService = new AtualizacaoService(contatoRepository, regiaoRepository, brasilGateway);
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

            await _cadastroService.Cadastrar(contatoExistente);

            var novoContato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _cadastroService.Cadastrar(novoContato));
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
            await _cadastroService.Cadastrar(contato);

            // Assert
            Assert.Contains(_context.Contatos, c => c.ContatoNome == contato.ContatoNome);
        }

        [Fact]
        public async Task Atualizar_DeveLancarException_QuandoContatoJaExiste()
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
            await Assert.ThrowsAsync<Exception>(() => _atualizacaoService.Atualizar(contato));
        }

        [Fact]
        public async Task Atualizar_DeveChamarRepository_QuandoContatoEhValido()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "1198563254",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            var regiao = new Regiao { Id = 1, RegiaoNome = "São Paulo", RegiaoDdd = 11 };
            await _context.Regioes.AddAsync(regiao);
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();

            // Altera telefone do contato
            contato.ContatoTelefone = "1132336598";

            // Act
            await _atualizacaoService.Atualizar(contato);

            // Assert
            var contatoAlterado = await _context.Contatos.FindAsync(contato.Id);
            Assert.Equal(contato.ContatoNome, contatoAlterado.ContatoNome);
        }

        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
