using Aplication.DTOs;
using Aplication.DTOs.Request.Profesor;
using Aplication.DTOs.Response.Profesor;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IProfesor
{
    public interface IProfesorCommand
    {
        Task<ProfesorDto> CreateAsync(ProfesorDto dto);
        Task<ProfesorDto?> UpdateAsync(int id, ProfesorDto dto);
        Task CreateProfesor(Profesor profesor);
        Task<ProfesorResponse?> UpdateSimpleAsync(int id, UpdateProfesorSimpleRequest request);
        Task<(bool Success, List<string> AffectedItems, string ProfesorName)> DeleteAsync(int id);
        Task<bool> AsignarClaseAsync(int profesorId, int claseId);
        Task<bool> AsignarEntrenamientoAsync(int profesorId, int entrenamientoId);
        Task<bool> ValidarCertificadoAsync(int profesorId);
    }
}
