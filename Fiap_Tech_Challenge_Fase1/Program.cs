using ConsultaContato.Extensions;
using Microsoft.AspNetCore.Builder;
using Prometheus;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.Inject(builder.Configuration);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configurar as métricas
        app.UseHttpMetrics();

        app.UseAuthorization();
        // Ordem correta dos middlewares
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Mapeia os controllers
            endpoints.MapMetrics(); // Configura o endpoint /metrics
        });

        app.UseHttpsRedirection();

        app.Run();
    }
}
