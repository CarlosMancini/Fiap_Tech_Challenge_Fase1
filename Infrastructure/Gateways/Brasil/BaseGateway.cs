namespace Infrastructure.Gateways.Brasil
{
    public class BaseGateway
    {
        public readonly HttpClient httpClient;
        public BaseGateway(string baseUrl)
        {
            httpClient = new()
            {
                BaseAddress = new Uri(baseUrl),
            };
        }
    }
}
