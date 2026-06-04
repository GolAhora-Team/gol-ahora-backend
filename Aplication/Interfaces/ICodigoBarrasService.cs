using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ICodigoBarrasService
    {
        Task<byte[]> GenerarPulseraPdfAsync(string codigoBarras, string nombreUsuario, string nombreActividad, string fechaActividad);
        Task<byte[]> GenerarPulseraPdfParaActividadAsync(int actividadId, int clienteId, bool esClase);
    }
}
