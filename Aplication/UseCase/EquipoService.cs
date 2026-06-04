using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.IUsuario;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class EquipoService : IEquipoService
    {
        private readonly IEquipoCommand _command;
        private readonly IEquipoQuery _query;
        private readonly IEquipoMapper _mapper;
        private readonly Aplication.Interfaces.INotificacionService _notificacionService;
        private readonly IUsuarioQuery _usuarioQuery;
        private readonly Aplication.Interfaces.IEmailService _emailService;
        private readonly Aplication.Interfaces.IJugador.IJugadorCommand _jugadorCommand;
        private readonly Aplication.Interfaces.IJugador.IJugadorQuery _jugadorQuery;

        public EquipoService(IEquipoCommand command, IEquipoQuery query, IEquipoMapper mapper, Aplication.Interfaces.INotificacionService notificacionService, IUsuarioQuery usuarioQuery, Aplication.Interfaces.IEmailService emailService, Aplication.Interfaces.IJugador.IJugadorCommand jugadorCommand, Aplication.Interfaces.IJugador.IJugadorQuery jugadorQuery)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
            _notificacionService = notificacionService;
            _usuarioQuery = usuarioQuery;
            _emailService = emailService;
            _jugadorCommand = jugadorCommand;
            _jugadorQuery = jugadorQuery;
        }

        public async Task<EquipoResponse> CreateEquipo(CreateEquipoRequest request)
        {
            var equipo = _mapper.CreateEquipo(request);

            await _command.InsertEquipo(equipo);

            await _notificacionService.CrearNotificacionGeneral(
                $"Nuevo equipo registrado: {equipo.Nombre}", 
                "ADMIN,PERSONAL", 
                "Equipo"
            );

            equipo = await _query.GetEquipoById(equipo.Id);
            return _mapper.CreateEquipoResponse(equipo);
        }

        public async Task<EquipoResponse> DeleteEquipo(int equipoId)
        {
            var cliente = await _query.GetEquipoById(equipoId);
            if (cliente == null)
            {
                throw new Exception("Equipo no encontrado");
            }

            await _command.RemoveEquipo(cliente.Id);
            return _mapper.CreateEquipoResponse(cliente);
        }

        public async Task<List<EquipoResponse>> GetAll()
        {
            var equipos = await _query.GetListEquipos();

            return equipos.Select(equipo => _mapper.CreateEquipoResponse(equipo)).ToList();
        }

        public async Task<EquipoResponse> GetEquipoById(int equipoId)
        {
            var equipo = await _query.GetEquipoById(equipoId);

            if (equipo == null)
                throw new Exception("El equipo no existe");

            return _mapper.CreateEquipoResponse(equipo);
        }

        public async Task<EquipoResponse> UpdateEquipo(int equipoId, CreateEquipoRequest request)
        {
            var equipoOriginal = await _query.GetEquipoById(equipoId);

            if (equipoOriginal == null)
                throw new Exception("El equipo no existe");

            equipoOriginal.Nombre = request.Nombre;
            equipoOriginal.CantidadMaxJugadores = request.CantidadMaxJugadores;
            equipoOriginal.Descripcion = request.Descripcion;
            equipoOriginal.ColorPrimario = request.ColorPrimario ?? equipoOriginal.ColorPrimario;
            equipoOriginal.ColorSecundario = request.ColorSecundario ?? equipoOriginal.ColorSecundario;
            equipoOriginal.CompeticionId = request.CompeticionId;

            await _command.UpdateEquipo(equipoOriginal);

            equipoOriginal = await _query.GetEquipoById(equipoOriginal.Id);
            return _mapper.CreateEquipoResponse(equipoOriginal);
        }

        public async Task<List<EquipoResponse>> GetEquiposByClienteId(int clienteId)
        {
            var equipos = await _query.GetEquiposByClienteId(clienteId);
            return equipos.Select(equipo => _mapper.CreateEquipoResponse(equipo)).ToList();
        }

        public async Task InvitarJugador(int equipoId, string username, int invitadoPorUsuarioId)
        {
            var equipo = await _query.GetEquipoById(equipoId);
            if (equipo == null)
                throw new Exception("El equipo no existe");

            // Buscar usuario por username
            var usuarioDestino = await _usuarioQuery.GetByUsername(username);
            if (usuarioDestino == null)
                throw new Exception("No se encontró un usuario con ese nombre de usuario");

            // Verificar que sea un cliente
            if (usuarioDestino.TipoUsuario != Domain.Enums.TipoUsuario.Cliente)
                throw new Exception("El usuario no es de tipo Cliente");

            // Enviar notificación de invitación
            var mensaje = $"Te han invitado a unirte al equipo \"{equipo.Nombre}\". ¿Querés aceptar?";
            var notificacion = await _notificacionService.CrearNotificacionInvitacion(
                mensaje,
                usuarioDestino.Id,
                equipoId,
                invitadoPorUsuarioId
            );

            // Enviar correo electrónico
            if (!string.IsNullOrEmpty(usuarioDestino.Email))
            {
                var htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='background-color: #ffffff; padding: 30px; border-radius: 10px; text-align: center;'>
                        <h2 style='color: #009b3a;'>¡Hola {usuarioDestino.Username}!</h2>
                        <p style='color: #333; font-size: 16px;'>Te han invitado a unirte al equipo <strong>{equipo.Nombre}</strong> en Complejo Gol Ahora.</p>
                        <p style='color: #333; font-size: 16px;'>Ingresá a la plataforma y dirígete a tus notificaciones para aceptar o rechazar la invitación.</p>
                        <br/>
                        <p style='color: #888; font-size: 12px;'>Complejo Gol Ahora - Sistema de Gestión Deportiva</p>
                    </div>
                </body>
                </html>";
                await _emailService.SendEmailAsync(usuarioDestino.Email, $"Invitación al equipo {equipo.Nombre} - Complejo Gol Ahora", htmlBody);
            }
        }

        public async Task<EquipoResponse> GuardarFormacion(int equipoId, UpdateFormacionRequest request)
        {
            var equipo = await _query.GetEquipoById(equipoId);
            if (equipo == null)
            {
                throw new Domain.Exceptions.ExceptionNotFound("El equipo no existe.");
            }

            // Find existing formation or create new
            var formacion = equipo.Formaciones?.FirstOrDefault(f => f.TipoCancha == request.TipoCancha);
            if (formacion == null)
            {
                formacion = new EquipoFormacion
                {
                    TipoCancha = request.TipoCancha,
                    FormacionDefecto = request.FormacionDefecto,
                    JugadoresPosiciones = new List<JugadorFormacion>()
                };
                if (equipo.Formaciones == null) equipo.Formaciones = new List<EquipoFormacion>();
                equipo.Formaciones.Add(formacion);
            }
            else
            {
                formacion.FormacionDefecto = request.FormacionDefecto;
            }

            // Map players
            if (request.Jugadores != null)
            {
                formacion.JugadoresPosiciones.Clear();
                foreach (var reqJugador in request.Jugadores)
                {
                    var jugador = await _jugadorQuery.GetJugadorById(reqJugador.JugadorId);
                    if (jugador != null && jugador.EquipoId == equipoId)
                    {
                        formacion.JugadoresPosiciones.Add(new JugadorFormacion
                        {
                            JugadorId = jugador.Id,
                            Posicion = reqJugador.Posicion,
                            EsTitular = reqJugador.EsTitular,
                            EsCapitan = (request.CapitanId.HasValue && jugador.Id == request.CapitanId.Value)
                        });
                    }
                }
            }

            await _command.UpdateEquipo(equipo);

            return _mapper.CreateEquipoResponse(equipo);
        }
    }
}
