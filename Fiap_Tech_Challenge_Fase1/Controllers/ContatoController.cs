using Core.Entities;
using Core.Inputs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap_Tech_Challenge_Fase1.Controllers
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

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ContatoInputCadastrar input)
        {
            try
            {
                var contato = new Contato()
                {
                    ContatoNome = input.ContatoNome,
                    ContatoTelefone = input.ContatoTelefone,
                    ContatoEmail = input.ContatoEmail
                };

                await _contatoService.Cadastrar(contato);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Alterar([FromBody] ContatoInputAtualizar input)
        {
            try
            {
                var contato = new Contato()
                {
                    Id = input.Id,
                    ContatoNome = input.ContatoNome,
                    ContatoTelefone = input.ContatoTelefone,
                    ContatoEmail = input.ContatoEmail
                };

                await _contatoService.Alterar(contato);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _contatoService.Deletar(Id);
                return Ok();
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
