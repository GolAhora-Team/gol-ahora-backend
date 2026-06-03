using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.INotificacion
{
    public interface INotificacionCommand
    {
        Task InsertNotificacion(Notificacion notificacion);
        Task MarcarComoLeidas(int usuarioId);
    }

    public interface INotificacionQuery
    {
        Task<List<Notificacion>> ObtenerNotificacionesPorUsuario(int usuarioId, string rol);
    }
}
