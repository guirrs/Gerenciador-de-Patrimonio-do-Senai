using GestaoPatrimonio.Aplication.Services;
using GestaoPatrimonio.DTOs.LogPatrimonioDto;
using GestaoPatrimonio.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogPatrimonioController : ControllerBase
    {
        private readonly LogPatrimonioService _service;

        public LogPatrimonioController(LogPatrimonioService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]  
        public ActionResult<List<ListarLogPatrimonioDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<List<ListarLogPatrimonioDto>> BuscarPorPatrimonio(Guid id)
        {
            try
            {
                return Ok(_service.ListarPorPatrimonio(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
