using Core.Entities;
using Core.Interfaces.Services;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ContatoAtualizadoConsumidor : IConsumer<Contato>
    {
        private readonly IAtualizacaoService _atualizacaoService;

        public ContatoAtualizadoConsumidor(IAtualizacaoService atualizacaoService)
        {
            _atualizacaoService = atualizacaoService;
        }

        public async Task Consume(ConsumeContext<Contato> context)
        {
            try
            {
                var contato = context.Message;
                await _atualizacaoService.Atualizar(contato);
                Console.WriteLine($"Contato {contato.ContatoNome} atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar contato: {ex.Message}");
            }
        }
    }
}
