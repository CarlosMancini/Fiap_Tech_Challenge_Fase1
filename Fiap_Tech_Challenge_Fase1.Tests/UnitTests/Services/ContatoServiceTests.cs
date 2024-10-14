using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Fiap_Tech_Challenge_Fase1.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Fiap_Tech_Challenge_Fase1.Tests.UnitTests.Services
{
    public class ContatoServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IContatoRepository _contatoRepository;
        private readonly IRegiaoRepository _regiaoRepository;
        private readonly IBrasilGateway _brasilGateway;
        private readonly IContatoService _contatoService;

        public ContatoServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _contatoRepository = new ContatoRepository(_context);
            _regiaoRepository = new RegiaoRepository(_context);
            _brasilGateway = new BrasilGateway();

            _contatoService = new ContatoService(_contatoRepository, _regiaoRepository, _brasilGateway);
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
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

            await _contatoRepository.Cadastrar(contatoExistente); // Cadastra o contato existente

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
            var novaRegiao = new Regiao
            {
                Id = 1,
                RegiaoNome = "São Paulo",
                RegiaoDdd = 11
            };

            // Cadastra a nova região
            await _regiaoRepository.Cadastrar(novaRegiao);

            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br",
                RegiaoId = novaRegiao.Id // Associa a região ao contato
            };

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            var contatos = await _contatoRepository.ObterTodos();
            Assert.Single(contatos); // Verifica se um contato foi cadastrado
        }

        //[Fact]
        //public async Task Cadastrar_DeveChamarCadastrarRegiao_QuandoRegiaoEhNova()
        //{
        //    // Arrange
        //    var contato = new Contato
        //    {
        //        ContatoNome = "Bruce Wayne",
        //        ContatoTelefone = "1123456789",
        //        ContatoEmail = "bruce.wayne@wayneltda.com.br"
        //    };

        //    int ddd = int.Parse(contato.ContatoTelefone[..2]);
        //    string regiaoNome = "São Paulo";

        //    // Simula que não há nenhuma região existente com o DDD informado
        //    var regiaoExistente = await _regiaoRepository.ObterPorDdd(ddd);
        //    Assert.Null(regiaoExistente); // Verifica que a região não existe

        //    // Simula a busca do nome da região baseada no DDD através do gateway
        //    _brasilGateway.Setup(g => g.BuscarDDDAsync(ddd)).ReturnsAsync(regiaoNome);

        //    // Act
        //    await _contatoService.Cadastrar(contato);

        //    // Assert
        //    var regiaoCadastrada = await _regiaoRepository.ObterPorDdd(ddd);
        //    Assert.NotNull(regiaoCadastrada); // Verifica se a nova região foi cadastrada
        //    Assert.Equal(regiaoNome, regiaoCadastrada.RegiaoNome); // Verifica se o nome está correto
        //    Assert.Equal(1, contato.RegiaoId); // Verifica se o ID da nova região foi atribuído corretamente ao contato
        //}

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

            await _contatoRepository.Cadastrar(existingContato); // Cadastra um contato existente

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _contatoService.Alterar(contato));
        }

        //[Fact]
        //public async Task Alterar_DeveChamarRepository_QuandoContatoEhValido()
        //{
        //    // Arrange
        //    var contato = new Contato
        //    {
        //        Id = 1,
        //        ContatoNome = "Bruce Wayne",
        //        ContatoTelefone = "12345678901",
        //        ContatoEmail = "bruce.wayne@wayneltda.com.br"
        //    };

        //    await _contatoRepository.Cadastrar(contato); // Cadastra o contato

        //    var regiaoLista = new List<Regiao>
        //    {
        //        new Regiao { Id = 1, RegiaoNome = "São Paulo", RegiaoDdd = 11 }
        //    };

        //    _regiaoRepository.Setup(repo => repo.ObterTodos()).ReturnsAsync(regiaoLista);

        //    // Act
        //    await _contatoService.Alterar(contato);

        //    // Assert
        //    _contatoRepository.Verify(repo => repo.Alterar(contato), Times.Once);
        //}
    }
}
