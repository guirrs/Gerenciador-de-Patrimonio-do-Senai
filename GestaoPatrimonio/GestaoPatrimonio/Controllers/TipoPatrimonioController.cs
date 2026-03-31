using GestaoPatrimonio.Aplication.Services;
using GestaoPatrimonio.DTOs.TipoPatromonio;
using GestaoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPatrimonioController : ControllerBase
    {
        private readonly TipoPatrimonioService _service;

        public TipoPatrimonioController(TipoPatrimonioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarTipoPatrimonioDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<ListarTipoPatrimonioDto> ObterPorId(Guid id)
        {
            try
            {
                return Ok(_service.ObterPorId(id));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar (CriarTipoPatrimonioDto dto)
        {
            try
            {
                _service.Adicionar(dto);
                return Ok();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult Atualizar(CriarTipoPatrimonioDto dto, Guid id)
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
