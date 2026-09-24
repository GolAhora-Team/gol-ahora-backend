using Aplication.Interfaces.IFactura;
using Aplication.Interfaces;
using Aplication.DTOs.Request.Factura;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _service;
        private readonly IEmailService _emailService;

        public FacturaController(IFacturaService service, IEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var facturas = await _service.GetAll();
            return Ok(facturas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var factura = await _service.GetById(id);
            return Ok(factura);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFacturaRequest request)
        {
            var result = await _service.CreateFactura(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFacturaRequest request)
        {
            var result = await _service.UpdateFactura(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteFactura(id);
            return Ok();
        }

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

        [HttpPost("enviar-email")]
        public async Task<IActionResult> EnviarFacturaEmail([FromBody] EnviarFacturaEmailRequest req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.ToEmail) || string.IsNullOrWhiteSpace(req.PdfBase64))
                {
                    return BadRequest(new { mensaje = "El email destinatario y el PDF son obligatorios." });
                }

                // Extraer el base64 si viene con prefijo (ej: data:application/pdf;base64,...)
                var base64Data = req.PdfBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }

                byte[] pdfBytes = Convert.FromBase64String(base64Data);

                string fileName = string.IsNullOrWhiteSpace(req.FileName) ? "Factura.pdf" : req.FileName;
                if (!fileName.EndsWith(".pdf")) fileName += ".pdf";

                await _emailService.SendEmailWithAttachmentAsync(req.ToEmail, req.Subject, req.Body, pdfBytes, fileName);

                return Ok(new { mensaje = "Email enviado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }

    public class EnviarFacturaEmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string PdfBase64 { get; set; }
        public string FileName { get; set; }
    }
}
