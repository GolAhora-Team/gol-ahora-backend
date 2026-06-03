using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface INotificacionService
    {
        Task<Notificacion> CrearNotificacionGeneral(string mensaje, string rolesDestino, string tipo);
        Task<Notificacion> CrearNotificacionUsuario(string mensaje, int usuarioDestinoId, string tipo);
        Task<IEnumerable<Notificacion>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarNotificacionesComoVistas(int usuarioId);
    }
}
