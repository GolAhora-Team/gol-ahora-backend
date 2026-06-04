using Aplication.Interfaces.IRecibo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReciboController : ControllerBase
    {
        private readonly IReciboService _reciboService;

        public ReciboController(IReciboService reciboService)
        {
            _reciboService = reciboService;
        }

        [HttpGet("GenerarPdf/{reservaId}")]
        public async Task<IActionResult> GenerarPdf(int reservaId)
        {
            try
            {
                var pdfBytes = await _reciboService.GenerarPdfRecibo(reservaId);
                return File(pdfBytes, "application/pdf", $"Recibo_{reservaId}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
