using Aplication.Interfaces.INotificacion;
using Domain.Entities;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class NotificacionCommand : INotificacionCommand
    {
        private readonly AppDbContext _context;

        public NotificacionCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertNotificacion(Notificacion notificacion)
        {
            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
        }

        public async Task MarcarComoLeidas(int usuarioId)
        {
            var noLeidas = await _context.Notificaciones
                .Where(n => n.UsuarioDestinoId == usuarioId && !n.Leida)
                .ToListAsync();

            foreach(var n in noLeidas)
            {
                n.Leida = true;
            }

            if (noLeidas.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}

namespace Infraestructure.Querys
{
    public class NotificacionQuery : INotificacionQuery
    {
        private readonly AppDbContext _context;

        public NotificacionQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notificacion>> ObtenerNotificacionesPorUsuario(int usuarioId, string rol)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioDestinoId == usuarioId)
                .OrderByDescending(n => n.FechaCreacion)
                .Take(50)
                .ToListAsync();
        }
    }
}
