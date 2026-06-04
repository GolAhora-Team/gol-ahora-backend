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
        Task<List<AsistenciaResponse>> GetAsistenciasPorClaseYFecha(int claseId, DateTime fecha);
        Task<bool> RegistrarAsistenciaManual(int actividadId, int clienteId, bool esClase);
        Task<bool> RegistrarAsistenciaCodigoBarras(string codigoBarras, int actividadId, bool esClase);
    }
}
