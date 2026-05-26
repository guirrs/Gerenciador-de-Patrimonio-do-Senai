using GestaoPatrimonios.Applications.Services;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios_v1.DTOs.StatusTransferenciaDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusTransferenciaController : ControllerBase
    {
        private readonly StatusTransferenciaService _service;

        public StatusTransferenciaController(StatusTransferenciaService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<ListarStatusTransferenciaDto>> Listar()
        {
            List<ListarStatusTransferenciaDto> statusTransferencia = _service.Listar();
            return Ok(statusTransferencia);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ListarStatusTransferenciaDto> BuscarPorId(Guid id)
        {
            try
            {
                ListarStatusTransferenciaDto statusTransferencia = _service.BuscarPorId(id);
                return Ok(statusTransferencia);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Coordenador")]
        [HttpPost]
        public ActionResult Adicionar(CriarStatusTransferenciaDto dto)
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
        public ActionResult Atualizar(Guid id, CriarStatusTransferenciaDto dto)
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
