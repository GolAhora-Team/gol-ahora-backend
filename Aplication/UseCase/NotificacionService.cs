using Aplication.Interfaces;
using Aplication.Interfaces.INotificacion;
using Aplication.Interfaces.IUsuario;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Aplication.UseCase
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionCommand _command;
        private readonly INotificacionQuery _query;
        private readonly IUsuarioQuery _usuarioQuery;
        private readonly IUsuarioCommand _usuarioCommand;

        public NotificacionService(INotificacionCommand command, INotificacionQuery query, IUsuarioQuery usuarioQuery, IUsuarioCommand usuarioCommand)
        {
            _command = command;
            _query = query;
            _usuarioQuery = usuarioQuery;
            _usuarioCommand = usuarioCommand;
        }

        public async Task<Notificacion> CrearNotificacionGeneral(string mensaje, string rolesDestino, string tipo)
        {
            var roles = rolesDestino.Split(',').Select(r => r.Trim()).ToList();
            var usuarios = await _usuarioQuery.GetUsersByRoles(roles);

            foreach (var u in usuarios)
            {
                var notificacion = new Notificacion
                {
                    Mensaje = mensaje,
                    UsuarioDestinoId = u.Id,
                    Tipo = tipo,
                    FechaCreacion = DateTime.UtcNow,
                    Leida = false
                };
                await _command.InsertNotificacion(notificacion);
            }

            // Devolver un dummy ya que puede haber creado varias
            return new Notificacion { Mensaje = mensaje, Tipo = tipo };
        }

        public async Task<Notificacion> CrearNotificacionUsuario(string mensaje, int usuarioDestinoId, string tipo)
        {
            var notificacion = new Notificacion
            {
                Mensaje = mensaje,
                UsuarioDestinoId = usuarioDestinoId,
                Tipo = tipo,
                FechaCreacion = DateTime.UtcNow,
                Leida = false
            };

            await _command.InsertNotificacion(notificacion);
            return notificacion;
        }

        public async Task<IEnumerable<Notificacion>> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            var usuario = await _usuarioQuery.GetById(usuarioId);
            if (usuario == null) return Enumerable.Empty<Notificacion>();

            string rol = usuario.TipoUsuario.ToString().ToUpper();
            if (rol == "ADMINISTRADOR") rol = "ADMIN";

            return await _query.ObtenerNotificacionesPorUsuario(usuarioId, rol);
        }

        public async Task MarcarNotificacionesComoVistas(int usuarioId)
        {
            await _command.MarcarComoLeidas(usuarioId);
        }
    }
}
