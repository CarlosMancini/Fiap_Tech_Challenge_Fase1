using Atualizacao.Services;
using AtualizacaoConsumidor;
using AtualizacaoConsumidor.Eventos;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using MassTransit;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var filaAtualizacao = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

        services.AddScoped<IAtualizacaoService, AtualizacaoService>();
        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IBrasilGateway, BrasilGateway>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }, ServiceLifetime.Scoped);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ContatoAtualizado>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ReceiveEndpoint(filaAtualizacao, e =>
                {
                    e.Consumer<ContatoAtualizado>(context);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
