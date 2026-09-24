using Aplication.DTOs.Request.Pago;
using Aplication.Interfaces.IPago;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _service;

        public PagoController(IPagoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pagos = await _service.GetAll();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pago = await _service.GetById(id);
            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePagoRequest request)
        {
            var result = await _service.CreatePago(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePagoRequest request)
        {
            var result = await _service.UpdatePago(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletePago(id);
            return Ok();
        }
    }
}
