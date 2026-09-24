namespace SistemaGolAhora.Controllers
{
    using Aplication.DTOs.Request.Descuento;
    using Aplication.Interfaces.IDescuento;

    using Microsoft.AspNetCore.Mvc;

    namespace SistemaGolAhora.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class DescuentoController : ControllerBase
        {
            private readonly IDescuentoService _service;

            public DescuentoController(IDescuentoService service)
            {
                _service = service;
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateDescuentoRequest request)
            {
                var result = await _service.CreateDescuento(request);
                return Ok(result);
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var result = await _service.GetAll();
                return Ok(result);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await _service.GetById(id);
                return Ok(result);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateDescuentoRequest request)
            {
                var result = await _service.UpdateDescuento(id, request);
                return Ok(result);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                await _service.DeleteDescuento(id);
                return Ok(new { message = "Descuento eliminado correctamente" });
            }

            [HttpGet("test")]
            public IActionResult Test()
            {
                return Ok("Funciona descuento");
            }
        }
    }
}
