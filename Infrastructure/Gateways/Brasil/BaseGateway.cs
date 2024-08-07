using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
