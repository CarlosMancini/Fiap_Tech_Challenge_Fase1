using Core.Entities;
using Core.Interfaces.Services;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ContatoCriadoConsumidor : IConsumer<Contato>
    {
        private readonly ICadastroService _cadastroService;

        public ContatoCriadoConsumidor(ICadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        public async Task Consume(ConsumeContext<Contato> context)
        {
            try
            {
                var contato = context.Message;
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
