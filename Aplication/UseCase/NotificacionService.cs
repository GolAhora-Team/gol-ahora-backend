using Aplication.Interfaces;
using Aplication.Interfaces.INotificacion;
using Aplication.Interfaces.IUsuario;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.IJugador;
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
        private readonly IEquipoQuery _equipoQuery;
        private readonly IJugadorCommand _jugadorCommand;

        public NotificacionService(INotificacionCommand command, INotificacionQuery query, IUsuarioQuery usuarioQuery, IUsuarioCommand usuarioCommand, IEquipoQuery equipoQuery, IJugadorCommand jugadorCommand)
        {
            _command = command;
            _query = query;
            _usuarioQuery = usuarioQuery;
            _usuarioCommand = usuarioCommand;
            _equipoQuery = equipoQuery;
            _jugadorCommand = jugadorCommand;
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

        public async Task<Notificacion> CrearNotificacionInvitacion(string mensaje, int usuarioDestinoId, int equipoId, int invitadoPorUsuarioId)
        {
            var notificacion = new Notificacion
            {
                Mensaje = mensaje,
                UsuarioDestinoId = usuarioDestinoId,
                Tipo = "InvitacionEquipo",
                FechaCreacion = DateTime.UtcNow,
                Leida = false,
                AccionRequerida = true,
                EquipoId = equipoId,
                EstadoAccion = "Pendiente",
                InvitadoPorUsuarioId = invitadoPorUsuarioId
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

        public async Task AceptarInvitacion(int notificacionId)
        {
            var notificacion = await _query.GetNotificacionById(notificacionId);
            if (notificacion == null)
                throw new Exception("Notificación no encontrada");

            if (notificacion.EstadoAccion != "Pendiente")
                throw new Exception("Esta invitación ya fue procesada");

            notificacion.EstadoAccion = "Aceptada";
            notificacion.AccionRequerida = false;
            notificacion.Leida = true;
            await _command.UpdateNotificacion(notificacion);

            // Notificar al creador del equipo y agregar el jugador
            if (notificacion.InvitadoPorUsuarioId.HasValue && notificacion.EquipoId.HasValue)
            {
                var equipo = await _equipoQuery.GetEquipoById(notificacion.EquipoId.Value);
                var usuarioAceptado = await _usuarioQuery.GetById(notificacion.UsuarioDestinoId);
                
                if (usuarioAceptado?.Persona != null)
                {
                    // Agregar el jugador al equipo
                    var jugador = new Jugador
                    {
                        ClienteId = usuarioAceptado.Persona.Id,
                        EquipoId = notificacion.EquipoId.Value,
                        EquipoNombre = equipo?.Nombre ?? "Equipo Desconocido",
                        EsCapitan = false,
                        EsTitular = false,
                        Estado = Domain.Enums.EstadoJugador.Disponible,
                        Posicion = 0
                    };
                    await _jugadorCommand.InsertJugador(jugador);
                }

                var nombreAceptado = usuarioAceptado?.Persona != null 
                    ? $"{usuarioAceptado.Persona.Nombre} {usuarioAceptado.Persona.Apellido}" 
                    : usuarioAceptado?.Username ?? "Un usuario";

                await CrearNotificacionUsuario(
                    $"{nombreAceptado} aceptó la invitación para unirse al equipo \"{equipo?.Nombre}\".",
                    notificacion.InvitadoPorUsuarioId.Value,
                    "InvitacionAceptada"
                );
            }
        }

        public async Task RechazarInvitacion(int notificacionId)
        {
            var notificacion = await _query.GetNotificacionById(notificacionId);
            if (notificacion == null)
                throw new Exception("Notificación no encontrada");

            if (notificacion.EstadoAccion != "Pendiente")
                throw new Exception("Esta invitación ya fue procesada");

            notificacion.EstadoAccion = "Rechazada";
            notificacion.AccionRequerida = false;
            notificacion.Leida = true;
            await _command.UpdateNotificacion(notificacion);

            // Notificar al creador del equipo
            if (notificacion.InvitadoPorUsuarioId.HasValue && notificacion.EquipoId.HasValue)
            {
                var equipo = await _equipoQuery.GetEquipoById(notificacion.EquipoId.Value);
                var usuarioRechazado = await _usuarioQuery.GetById(notificacion.UsuarioDestinoId);
                var nombreRechazado = usuarioRechazado?.Persona != null 
                    ? $"{usuarioRechazado.Persona.Nombre} {usuarioRechazado.Persona.Apellido}" 
                    : usuarioRechazado?.Username ?? "Un usuario";

                await CrearNotificacionUsuario(
                    $"{nombreRechazado} rechazó la invitación para unirse al equipo \"{equipo?.Nombre}\".",
                    notificacion.InvitadoPorUsuarioId.Value,
                    "InvitacionRechazada"
                );
            }
        }
    }
}
