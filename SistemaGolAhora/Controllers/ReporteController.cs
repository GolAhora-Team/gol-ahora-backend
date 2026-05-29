using Aplication.Interfaces.IReporte;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportes()
        {
            try
            {
                var reportes = await _reporteService.GetAllReportes();
                return Ok(reportes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarReporte([FromBody] ReporteRequest request)
        {
            if (string.IsNullOrEmpty(request.Html))
            {
                return BadRequest("El contenido HTML es requerido");
            }

            try
            {
                var reporte = new Reporte
                {
                    Id = Guid.NewGuid(),
                    Fecha = DateTime.UtcNow,
                    FileName = request.FileName ?? $"Reporte-{DateTime.UtcNow:yyyyMMdd}",
                    Html = request.Html
                };

                await _reporteService.SaveReporte(reporte);
                return CreatedAtAction(nameof(GetReportes), new { id = reporte.Id }, reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al guardar el reporte", error = ex.Message });
            }
        }
    }

    public class ReporteRequest
    {
        public string FileName { get; set; }
        public string Html { get; set; }
    }
}
