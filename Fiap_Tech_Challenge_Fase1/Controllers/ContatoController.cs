using Core.Entities;
using Core.Inptus;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Fiap_Tech_Challenge_Fase1.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ContatoInput input)
        {
            try
            {
                var contato = new Contato()
                {
                    ContatoNome = input.ContatoNome,
                    ContatoTelefone = input.ContatoTelefone,
                    ContatoEmail = input.ContatoEmail
                };

                // To do: criar service com lógica de identificação do DDD da região a partir do telefone recebido

                _contatoRepository.Cadastrar(contato);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
