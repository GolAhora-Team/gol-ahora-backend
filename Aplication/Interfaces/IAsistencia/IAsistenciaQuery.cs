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
        Task<List<AsistenciaEntrenamiento>> GetAsistenciasEntrenamientoPorFecha(int entrenamientoId, DateTime fecha);
        
        Task<Asistencia?> GetInscripcionClaseAsync(int claseId, int clienteId);
        Task<Asistencia?> GetInscripcionClaseByBarcodeAsync(int claseId, string barcode);
        Task<ClienteEntrenamiento?> GetInscripcionEntrenamientoAsync(int entrenamientoId, int clienteId);
        Task<ClienteEntrenamiento?> GetInscripcionEntrenamientoByBarcodeAsync(int entrenamientoId, string barcode);
        Task<bool> YaAsistioEntrenamientoAsync(int entrenamientoId, int clienteId, DateTime fecha);
    }
}
