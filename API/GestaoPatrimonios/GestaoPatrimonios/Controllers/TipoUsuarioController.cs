using GestaoPatrimonios.Applications.Services;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios_v1.DTOs.TipoUsuarioDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonios.Controllers
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
            List<ListarTipoUsuarioDto> tiposUsuario = _service.Listar();
            return Ok(tiposUsuario);
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoUsuarioDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarTipoUsuarioDto tipoUsuario = _service.BuscarPorId(id);
                return Ok(tipoUsuario);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coordenador")]
        public ActionResult Adicionar(CriarTipoUsuarioDto dto)
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
        [Authorize(Roles = "Coordenador")]
        public ActionResult Atualizar(Guid id, CriarTipoUsuarioDto dto)
        {
            try
            {
                _service.Atualizar(id, dto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
