using Core.Interfaces.Services;
using Core.Mensagens;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ContatoExcluidoConsumidor : IConsumer<ContatoExcluidoMensagem>
    {
        private readonly IExclusaoService _exclusaoService;

        public ContatoExcluidoConsumidor(IExclusaoService exclusaoService)
        {
            _exclusaoService = exclusaoService;
        }

        public async Task Consume(ConsumeContext<ContatoExcluidoMensagem> context)
        {
            try
            {
                var contatoId = context.Message.Id;
                await _exclusaoService.Excluir(contatoId);
                Console.WriteLine($"Contato com ID {contatoId} excluído com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir contato: {ex.Message}");
            }
        }

    }
}
