using Core.Mensagens;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ExclusaoProdutor.Controllers
{
    [ApiController]
    [Route("/Exclusao")]
    public class ExclusaoController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public ExclusaoController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ContatoExcluidoMensagem input)
        {
            try
            {
                var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

                await endpoint.Send(input);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
