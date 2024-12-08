using Core.Entities;
using Core.Interfaces.Services;
using MassTransit;
using Produtor.Mensagens;

namespace Consumidor.Eventos
{
    public class ContatoCriadoConsumidor : IConsumer<ContatoCriadoMensagem>
    {
        private readonly ICadastroService _cadastroService;

        public ContatoCriadoConsumidor(ICadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        public async Task Consume(ConsumeContext<ContatoCriadoMensagem> context)
        {
            try
            {
                Contato contato = new Contato()
                {
                    ContatoNome = context.Message.ContatoNome,
                    ContatoTelefone = context.Message.ContatoTelefone,
                    ContatoEmail = context.Message.ContatoEmail
                };

                await _cadastroService.Cadastrar(contato);
                Console.WriteLine($"Contato {contato.ContatoNome} cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar contato: {ex.Message}");
            }
        }
    }
}
