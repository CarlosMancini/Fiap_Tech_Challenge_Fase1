using Cadastro.Services;
using CadastroConsumidor;
using CadastroConsumidor.Eventos;
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
        var filaCadastro = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

        services.AddScoped<ICadastroService, CadastroService>();
        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IBrasilGateway, BrasilGateway>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }, ServiceLifetime.Scoped);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ContatoCriado>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ReceiveEndpoint(filaCadastro, e =>
                {
                    e.Consumer<ContatoCriado>(context);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
