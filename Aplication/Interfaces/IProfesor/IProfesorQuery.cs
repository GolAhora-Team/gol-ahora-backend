using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IProfesor
{
    public interface IProfesorQuery
    {
        Task<IEnumerable<ProfesorDto>> GetAllAsync();
        Task<ProfesorDto?> GetByIdAsync(int id);
        Task<ProfesorDto?> GetByDniAsync(int dni);
        Task<IEnumerable<ProfesorDto>> GetByEspecialidadAsync(string especialidad);
        Task<IEnumerable<ProfesorDto>> GetByClaseAsync(int claseId);
        Task<byte[]?> GetCertificadoAsync(int profesorId);
    }
}
