using Aplication.DTOs.Request.Asistencia;
using Aplication.DTOs.Response.Asistencia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IAsistencia
{
    public interface IAsistenciaService
    {
        Task<List<AsistenciaResponse>> MarcarAsistencia(MarcarAsistenciaRequest request);
        Task<List<AsistenciaResponse>> GetAsistenciasPorActividadYFecha(int actividadId, DateTime fecha, bool esClase);
        Task<bool> RegistrarAsistenciaManual(int actividadId, int clienteId, bool esClase);
        Task<bool> RegistrarAsistenciaCodigoBarras(string codigoBarras, int actividadId, bool esClase);
        Task<bool> EliminarAsistencia(int actividadId, int clienteId, bool esClase);
        Task<List<AsistenciaResponse>> GetHistorialAsistencias(int actividadId, int clienteId, bool esClase);
    }
}
