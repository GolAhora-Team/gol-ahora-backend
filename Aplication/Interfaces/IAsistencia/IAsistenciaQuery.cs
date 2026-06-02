using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IAsistencia
{
    public interface IAsistenciaQuery
    {
        Task<Asistencia?> GetAsistenciaById(int id);

        Task<bool> AsistenciaExists(int id);

        Task<List<Asistencia>> GetAllAsistencias();
        
        Task<List<Asistencia>> GetAsistenciasPorClaseYFecha(int claseId, DateTime fecha);
    }
}
