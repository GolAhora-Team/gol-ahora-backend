using Aplication.Interfaces.IFactura;
using Aplication.DTOs.Request.Factura;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _service;

        public FacturaController(IFacturaService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var facturas = await _service.GetAll();
            return Ok(facturas);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var factura = await _service.GetById(id);
            return Ok(factura);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(CreateFacturaRequest request)
        {
            var result = await _service.CreateFactura(request);
            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFacturaRequest request)
        {
            var result = await _service.UpdateFactura(id, request);
            return Ok(result);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteFactura(id);
            return Ok();
        }

        // GET BY CLIENTE
        [HttpGet("por-cliente/{clienteId}")]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            try
            {
                var facturas = await _service.GetFacturasByClienteId(clienteId);
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
