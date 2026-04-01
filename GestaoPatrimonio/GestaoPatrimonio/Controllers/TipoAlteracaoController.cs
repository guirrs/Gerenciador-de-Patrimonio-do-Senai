using GestaoPatrimonio.Aplication.Services;
using GestaoPatrimonio.DTOs.TipoAlteracao;
using GestaoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoAlteracaoController : ControllerBase
    {
        private readonly TipoAlteracaoService _service;

        public TipoAlteracaoController(TipoAlteracaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarTipoAlteracao>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoAlteracao> ObterPorId(Guid id)
        {
            try
            {
                return Ok(_service.ObterPorId(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(CriarTipoAlteracao dto)
        {
            try
            {
                _service.Adicionar(dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public ActionResult Atualizar(CriarTipoAlteracao dto, Guid id)
        {
            try
            {
                _service.Atualizar(dto, id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
