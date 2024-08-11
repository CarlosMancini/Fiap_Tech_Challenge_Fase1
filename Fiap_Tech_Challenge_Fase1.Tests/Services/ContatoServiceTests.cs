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
        public async Task Cadastrar_DeveLancarException_QuandoContatoJaExiste()
        {
            // Arrange
            var mockContatoRepository = new Mock<IContatoRepository>();
            var mockRegiaoRepository = new Mock<IRegiaoRepository>();
            var mockBrasilGateway = new Mock<IBrasilGateway>();

            var contatoExistente = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            mockContatoRepository
                .Setup(r => r.ObterPorNomeETelefone(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(contatoExistente);

            var contatoService = new ContatoService(
                mockContatoRepository.Object,
                mockRegiaoRepository.Object,
                mockBrasilGateway.Object
            );

            var novoContato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => contatoService.Cadastrar(novoContato));
            Assert.Equal("Já existe um contato com o mesmo nome e telefone.", exception.Message);
        }

        [Fact]
        public async Task Cadastrar_DeveChamarRepository_QuandoContatoEhValido()
        {
            // Arrange
            var mockContatoRepository = new Mock<IContatoRepository>();
            var mockRegiaoRepository = new Mock<IRegiaoRepository>();
            var mockBrasilGateway = new Mock<IBrasilGateway>();

            var novaRegiao = new Regiao
            {
                Id = 1,
                RegiaoNome = "São Paulo",
                RegiaoDdd = 11
            };

            // Configura o mock para simular o retorno da lista de regiões antes do cadastro
            mockRegiaoRepository
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(new List<Regiao>());

            // Configura o mock para simular o cadastro da nova região
            mockRegiaoRepository
                .Setup(r => r.Cadastrar(It.IsAny<Regiao>()))
                .Callback<Regiao>(regiao =>
                {
                    regiao.Id = novaRegiao.Id; // Simula a atribuição do ID após o cadastro
                })
                .Returns(Task.CompletedTask);

            // Configura o mock para simular o retorno da lista de regiões após o cadastro
            mockRegiaoRepository
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(new List<Regiao> { novaRegiao });

            mockBrasilGateway
                .Setup(g => g.BuscarDDDAsync(It.IsAny<int>()))
                .ReturnsAsync("São Paulo");

            var contatoService = new ContatoService(
                mockContatoRepository.Object,
                mockRegiaoRepository.Object,
                mockBrasilGateway.Object
            );

            var contato = new Contato
            {
                ContatoNome = "Bruce Wayne",
                ContatoTelefone = "11999999999",
                ContatoEmail = "bruce.wayne@wayneltda.com.br"
            };

            // Act
            await contatoService.Cadastrar(contato);

            // Assert
            mockContatoRepository.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
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

            int ddd = Int32.Parse(contato.ContatoTelefone[..2]);
            string regiaoNome = "São Paulo";

            // Simula que não há nenhuma região existente com o DDD informado
            _regiaoRepositoryMock
                .SetupSequence(r => r.ObterTodos())
                .ReturnsAsync(new List<Regiao>()) // Primeira chamada: Nenhuma região existe
                .ReturnsAsync(new List<Regiao> { new Regiao { Id = 1, RegiaoDdd = ddd, RegiaoNome = regiaoNome } }); // Segunda chamada: Nova região cadastrada

            // Simula a busca do nome da região baseada no DDD através do gateway
            _brasilGatewayMock
                .Setup(g => g.BuscarDDDAsync(ddd))
                .ReturnsAsync(regiaoNome);

            // Simula o comportamento de cadastrar uma nova região e definir o ID da nova região
            _regiaoRepositoryMock
                .Setup(r => r.Cadastrar(It.IsAny<Regiao>()))
                .Callback<Regiao>(r => r.Id = 1) // Define um ID fictício para a nova região
                .Returns(Task.CompletedTask);

            // Act
            await _contatoService.Cadastrar(contato);

            // Assert
            _regiaoRepositoryMock.Verify(r => r.Cadastrar(It.IsAny<Regiao>()), Times.Once);
            Assert.Equal(1, contato.RegiaoId); // Verifica se o ID da nova região foi atribuído corretamente ao contato
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
            _contatoRepositoryMock.Setup(repo => repo.ObterPorNomeETelefone(contato.ContatoNome, contato.ContatoTelefone))
                                  .ReturnsAsync(existingContato);

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
    }
}
