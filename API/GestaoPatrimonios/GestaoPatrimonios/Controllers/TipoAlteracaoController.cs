using GestaoPatrimonios.Applications.Services;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios_v1.DTOs.TipoAlteracaoDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonios.Controllers
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

        [Authorize]
        [HttpGet]
        public ActionResult<List<ListarTipoAlteracaoDto>> Listar()
        {
            List<ListarTipoAlteracaoDto> tiposAlteracao = _service.Listar();
            return Ok(tiposAlteracao);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ListarTipoAlteracaoDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarTipoAlteracaoDto tipoAlteracao = _service.BuscarPorId(id);
                return Ok(tipoAlteracao);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Coordenador")]
        [HttpPost]
        public ActionResult Adicionar(CriarTipoAlteracaoDto dto)
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

        [Authorize(Roles = "Coordenador")]
        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarTipoAlteracaoDto dto)
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
