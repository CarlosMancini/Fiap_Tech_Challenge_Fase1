using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Fiap_Tech_Challenge_Fase1.Services;
using Moq;

namespace Fiap_Tech_Challenge_Fase1.Tests.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly Mock<IRegiaoRepository> _regiaoRepositoryMock;
        private readonly Mock<IBrasilGateway> _brasilGatewayMock;
        private readonly IContatoService _contatoService;

        public ContatoServiceTests()
        {
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _regiaoRepositoryMock = new Mock<IRegiaoRepository>();
            _brasilGatewayMock = new Mock<IBrasilGateway>();
            _contatoService = new ContatoService(_contatoRepositoryMock.Object, _regiaoRepositoryMock.Object, _brasilGatewayMock.Object);
        }

        [Fact]
        public async Task Cadastrar_ShouldThrowException_WhenContatoAlreadyExists()
        {
            // Arrange
            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };
            _contatoRepositoryMock.Setup(repo => repo.ObterPorNomeETelefone(contato.ContatoNome, contato.ContatoTelefone))
                                  .ReturnsAsync(contato);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _contatoService.Cadastrar(contato));
        }

        [Fact]
        public async Task Cadastrar_ShouldCallRepository_WhenContatoIsValid()
        {
            // Arrange
            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };
            _contatoRepositoryMock.Setup(repo => repo.ObterPorNomeETelefone(contato.ContatoNome, contato.ContatoTelefone))
                                  .ReturnsAsync((Contato)null);

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            _contatoRepositoryMock.Verify(repo => repo.Cadastrar(contato), Times.Once);
        }

        [Fact]
        public async Task Alterar_ShouldThrowException_WhenContatoAlreadyExists()
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
            _contatoRepositoryMock.Setup(repo => repo.ObterPorNomeETelefone(contato.ContatoNome, contato.ContatoTelefone))
                                  .ReturnsAsync(existingContato);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _contatoService.Alterar(contato));
        }

        [Fact]
        public async Task Alterar_ShouldCallRepository_WhenContatoIsValid()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            var regiaoLista = new List<Regiao>
            {
                new Regiao { Id = 1, RegiaoNome = "São Paulo", RegiaoDdd = 11 }
            };

            _regiaoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(regiaoLista);

            _regiaoRepositoryMock.Setup(repo => repo.Cadastrar(It.IsAny<Regiao>()))
                .Callback<Regiao>(r =>
                {
                    r.Id = 2; // Simulando que o ID é gerado após o cadastro
                    regiaoLista.Add(r);
                });


            // Act
            await _contatoService.Alterar(contato);

            // Assert
            _contatoRepositoryMock.Verify(repo => repo.Alterar(contato), Times.Once);
        }

        [Fact]
        public async Task Cadastrar_ShouldCallCadastrarRegiao_WhenRegiaoIsNew()
        {
            // Arrange
            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "12345678901",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };
            var regiao = new Regiao 
            { 
                RegiaoDdd = 12,
                RegiaoNome = "São Paulo Interior"
            };
            _regiaoRepositoryMock.Setup(repo => repo.ObterTodos()).ReturnsAsync(new List<Regiao>());
            _brasilGatewayMock.Setup(gateway => gateway.BuscarDDDAsync(It.IsAny<int>())).ReturnsAsync("New Region");

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            _regiaoRepositoryMock.Verify(repo => repo.Cadastrar(It.IsAny<Regiao>()), Times.Once);
        }
    }
}
