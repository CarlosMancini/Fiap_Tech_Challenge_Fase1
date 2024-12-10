using Core.Entities;
using Core.Interfaces.Services;
using Core.Mensagens;
using MassTransit;

namespace AtualizacaoConsumidor.Eventos
{
    public class ContatoAtualizado : IConsumer<ContatoAtualizadoMensagem>
    {
        private readonly IAtualizacaoService _atualizacaoService;

        public ContatoAtualizado(IAtualizacaoService atualizacaoService)
        {
            _atualizacaoService = atualizacaoService;
        }

        public async Task Consume(ConsumeContext<ContatoAtualizadoMensagem> context)
        {
            try
            {
                Contato contato = new Contato
                {
                    Id = context.Message.Id,
                    ContatoNome = context.Message.ContatoNome,
                    ContatoTelefone = context.Message.ContatoTelefone,
                    ContatoEmail = context.Message.ContatoEmail
                };

                await _atualizacaoService.Atualizar(contato);
                Console.WriteLine($"Contato {contato.ContatoNome} atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar contato: {ex.Message}");
            }
        }
    }
}
