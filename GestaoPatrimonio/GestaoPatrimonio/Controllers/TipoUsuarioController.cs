using GestaoPatrimonio.Aplication.Services;
using GestaoPatrimonio.DTOs.TipoUsuarioDto;
using GestaoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly TipoUsuarioService _service;

        public TipoUsuarioController(TipoUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarTipoUsuarioDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoUsuarioDto> ObterPorId(Guid id)
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
        public ActionResult Adicionar(CriarTipoUsuariodto dto)
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
        public ActionResult Atualizar(CriarTipoUsuariodto dto, Guid id)
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
