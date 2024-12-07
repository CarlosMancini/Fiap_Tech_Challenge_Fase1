using Consumidor;
using Consumidor.Eventos;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var filaCadastro = configuration.GetSection("MassTransit")["NomeFilaCadastro"] ?? string.Empty;
        var filaAtualizacao = configuration.GetSection("MassTransit")["NomeFilaAtualizacao"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });
                cfg.ReceiveEndpoint(filaCadastro, e =>
                {
                    e.Consumer<ContatoCriadoConsumidor>(context);
                });
                cfg.ReceiveEndpoint(filaAtualizacao, e =>
                {
                    e.Consumer<ContatoAtualizadoConsumidor>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<ContatoCriadoConsumidor>();
            x.AddConsumer<ContatoAtualizadoConsumidor>();
        });
    })
    .Build();

host.Run();