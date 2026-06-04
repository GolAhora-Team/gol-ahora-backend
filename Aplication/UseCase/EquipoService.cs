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

        public EquipoService(IEquipoCommand command, IEquipoQuery query, IEquipoMapper mapper, Aplication.Interfaces.INotificacionService notificacionService, IUsuarioQuery usuarioQuery)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
            _notificacionService = notificacionService;
            _usuarioQuery = usuarioQuery;
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
        }
    }
}
