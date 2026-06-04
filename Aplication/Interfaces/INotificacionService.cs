using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface INotificacionService
    {
        Task<Notificacion> CrearNotificacionGeneral(string mensaje, string rolesDestino, string tipo);
        Task<Notificacion> CrearNotificacionUsuario(string mensaje, int usuarioDestinoId, string tipo);
        Task<Notificacion> CrearNotificacionInvitacion(string mensaje, int usuarioDestinoId, int equipoId, int invitadoPorUsuarioId);
        Task<IEnumerable<Notificacion>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarNotificacionesComoVistas(int usuarioId);
        Task AceptarInvitacion(int notificacionId);
        Task RechazarInvitacion(int notificacionId);
    }
}
