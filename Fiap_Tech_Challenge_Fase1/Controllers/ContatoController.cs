using Fiap_Tech_Challenge_Fase1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fiap_Tech_Challenge_Fase1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoCadastro _contatoCadastro;

        public ContatoController(IContatoCadastro contatoCadastro)
        {
            _contatoCadastro = contatoCadastro;
        }

        [HttpGet]
        public IActionResult GetContatos()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CriarContato()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult AtualizarContato()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeletarContato()
        {
            return Ok();
        }
    }
}
