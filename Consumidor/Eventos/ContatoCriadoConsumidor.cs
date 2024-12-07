using Core.Entities;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ContatoCriadoConsumidor : IConsumer<Contato>
    {
        public Task Consume(ConsumeContext<Contato> context)
        {
            Console.WriteLine(context.Message);
            return Task.CompletedTask;
        }
    }
}
