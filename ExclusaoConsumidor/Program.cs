using Core.Interfaces.Services;
using Exclusao.Services;
using ExclusaoConsumidor;
using ExclusaoConsumidor.Eventos;
using Infrastructure.Database.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var filaExclusao = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

        services.AddScoped<IExclusaoService, ExclusaoService>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }, ServiceLifetime.Scoped);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ContatoExcluido>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ReceiveEndpoint(filaExclusao, e =>
                {
                    e.Consumer<ContatoExcluido>(context);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
