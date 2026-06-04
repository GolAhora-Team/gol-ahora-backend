using Aplication.DTOs;
using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Response;
using Aplication.Interfaces.IJugador;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class JugadoresService : IJugadorService
    {
        private readonly IJugadorMapper _mapper;
        private readonly IJugadorQuery _query;
        private readonly IJugadorCommand _command;

        public JugadoresService(IJugadorMapper mapper, IJugadorQuery query, IJugadorCommand command)
        {
            _mapper = mapper;
            _query = query;
            _command = command;
        }

        public async Task<JugadorResponse> CreateJugador(JugadorRequest request)
        {
            var jugador = _mapper.CreateJugador(request);
            await _command.InsertJugador(jugador);

            jugador = await _query.GetJugadorById(jugador.Id);
            return _mapper.CreateJugadorResponse(jugador);
        }

        public async Task<JugadorResponse> DeleteJugador(int jugadorId)
        {
            var jugador = await _query.GetJugadorById(jugadorId);
            if (jugador == null)
            {
                throw new Exception("Jugador no encontrado");
            }

            await _command.RemoveJugador(jugador.Id);
            return _mapper.CreateJugadorResponse(jugador);
        }

        public async Task<List<JugadorResponse>> GetAll()
        {
            var jugadores = await _query.GetListJugadores();

            return jugadores.Select(jugador => _mapper.CreateJugadorResponse(jugador)).ToList();
        }

        public async Task<JugadorResponse> GetJugadorById(int jugadorId)
        {
            var jugador = await _query.GetJugadorById(jugadorId);

            if (jugador == null)
                throw new Exception("El jugador no existe");

            return _mapper.CreateJugadorResponse(jugador);
        }

        public async Task<JugadorResponse> UpdateJugador(int jugadorId, JugadorRequest request)
        {
            var jugadorOriginal = await _query.GetJugadorById(jugadorId);

            if (jugadorOriginal == null)
                throw new Exception("El jugador no existe");

            jugadorOriginal.Numero = request.Numero;
            jugadorOriginal.ClienteId = request.ClienteId;
            jugadorOriginal.EquipoId = request.EquipoId;

            await _command.UpdateJugador(jugadorOriginal);

            jugadorOriginal = await _query.GetJugadorById(jugadorOriginal.Id);
            return _mapper.CreateJugadorResponse(jugadorOriginal);
        }
    }
}
