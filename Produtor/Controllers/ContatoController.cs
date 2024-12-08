using Core.Entities;
using Core.Inputs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Controllers
{
    [ApiController]
    [Route("/Contato")]
    public class ContatoController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public ContatoController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarContato([FromBody] ContatoInputCadastrar input)
        {
            try
            {
                var nomeFila = _configuration.GetSection("MassTransit")["NomeFilaCadastro"] ?? string.Empty;
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

                var contato = new Contato
                {
                    ContatoNome = input.ContatoNome,
                    ContatoEmail = input.ContatoEmail,
                    ContatoTelefone = input.ContatoTelefone
                };

                await endpoint.Send(contato);
                return Ok("Contato enviado para a fila.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarContato([FromBody] ContatoInputAtualizar input)
        {
            try
            {
                var nomeFila = _configuration.GetSection("MassTransit")["NomeFilaAtualizacao"] ?? string.Empty;
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

                var contato = new Contato()
                {
                    Id = input.Id,
                    ContatoNome = input.ContatoNome,
                    ContatoTelefone = input.ContatoTelefone,
                    ContatoEmail = input.ContatoEmail
                };

                await endpoint.Send(contato);
                return Ok("Contato enviado para a fila.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
