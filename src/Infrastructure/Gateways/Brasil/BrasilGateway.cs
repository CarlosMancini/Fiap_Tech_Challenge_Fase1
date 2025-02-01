using System.Net.Http.Json;
using Core.Gateways;

namespace Infrastructure.Gateways.Brasil
{
    public class BrasilGateway : BaseGateway, IBrasilGateway
    {
        public BrasilGateway() : base("https://brasilapi.com.br/api/")
        {
        }

        public async Task<string> BuscarDDDAsync(int ddd)
        {
            var response = await this.httpClient.GetFromJsonAsync<DDDResponse>($"ddd/v1/{ddd}");

            if (response == null) throw new Exception("DDD não encontrado");
            
            var nomeEstado = TradutorEstado.TraduzirSigla(response.State);
            return nomeEstado;
        }
    }

    public record DDDResponse
    {
        public string State { get; init; }
        public List<string> Cities { get; init; }
    }
}
