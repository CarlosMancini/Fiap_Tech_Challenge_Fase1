using Consumidor;
using Consumidor.Eventos;
using Core.Interfaces.Services;
using Cadastro.Services;
using Atualizacao.Services;
using MassTransit;
using Core.Gateways;
using Core.Interfaces.Repository;
using Infrastructure.Database.Repository;
using Infrastructure.Gateways.Brasil;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var filaCadastro = configuration.GetSection("MassTransit")["NomeFilaCadastro"] ?? string.Empty;
        var filaAtualizacao = configuration.GetSection("MassTransit")["NomeFilaAtualizacao"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

        services.AddScoped<ICadastroService, CadastroService>();
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
            x.AddConsumer<ContatoCriadoConsumidor>();
            x.AddConsumer<ContatoAtualizadoConsumidor>();

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
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
