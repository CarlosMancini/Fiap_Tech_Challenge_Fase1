using System.Net.Http.Json;
using Core.Gateways;

namespace Infrastructure.Gateways.Brasil
{
    public class BrasilGateway() : BaseGateway("https://brasilapi.com.br/api/"), IBrasilGateway
    {
        public async Task<string> BuscarDDDAsync(int ddd)
        {
            var response = await this.httpClient.GetFromJsonAsync<DDDResponse>($"ddd/v1/{ddd}");

            return response == null ? throw new Exception("ddd não encontrado") : response.state;
        }
    }

    record DDDResponse {
        public string state;
        public List<string> cities;
    }
}
