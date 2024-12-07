using Core.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Controllers
{
    [ApiController]
    [Route("/Cadastro")]
    public class CadastroController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public CadastroController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarContato(/*[FromBody] ContatoInputCadastrar contato*/)
        {
            var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

            var contato = new Contato { ContatoNome = "Cadu", ContatoEmail = "cadu@cadu.com", ContatoTelefone = "61956833569" };

            await endpoint.Send(contato);

            //await _cadastroService.CadastrarContato(contato);
            return Ok("Contato enviado para a fila.");
        }
    }
}
