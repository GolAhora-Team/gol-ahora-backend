using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Request.Equipo;
using Aplication.Interfaces.ICompeticion;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompeticionController : ControllerBase
    {
        private readonly ICompeticionService _services;

        public CompeticionController(ICompeticionService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GenerarCompeticion([FromBody] CompeticionRequest request)
        {
            try
            {
                var result = await _services.CreateCompeticion(request);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var competicion = await _services.GetCompeticionById(id);
                return Ok(competicion);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompeticionRequest request)
        {
            try
            {
                var response = await _services.UpdateCompeticion(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.DeleteCompeticion(id);
                return Ok(new { mensaje = "Competición eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
