using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaContato.Controllers
{
    [ApiController]
    [Route("contacts")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;

        public ContatoController(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [HttpGet("obter-todos")]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var result = await _contatoService.ObterTodos();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> ObterPorId(int Id)
        {
            try
            {
                var result = await _contatoService.ObterPorId(Id);
                return result != null ? Ok(result) : NotFound(); // Retorna NotFound se não existir
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("regions/{Id}")]
        public IActionResult ObterPorRegiao(int Id)
        {
            try
            {
                var result = _contatoService.ObterPorRegiao(Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
