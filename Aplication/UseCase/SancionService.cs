using Aplication.DTOs.Request.Sancion;
using Aplication.DTOs.Response;
using Aplication.Interfaces.IJugador;
using Aplication.Interfaces.ISancion;
using Aplication.Mappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class SancionService : ISancionService
    {
        private readonly ISancionMapper _sancionMapper;
        private readonly ISancionCommand _sancionCommand;
        private readonly ISancionQuery _sancionQuery;
        private readonly IJugadorQuery _jugadorQuery;

        public SancionService(ISancionMapper sancionMapper, ISancionCommand sancionCommand,ISancionQuery sancionQuery, IJugadorQuery jugadorQuery)
        {
            _sancionMapper = sancionMapper;
            _sancionCommand = sancionCommand;
            _sancionQuery = sancionQuery;
            _jugadorQuery = jugadorQuery;
        }

        public async Task<SancionResponse> CreateSancion(SancionRequest request)
        {            
            var jugador = await _jugadorQuery.GetJugadorById(request.JugadorId);
            if (jugador == null)
            {                
                throw new Exception($"No se puede registrar la sanción: El jugador con ID {request.JugadorId} no existe.");
            }
                        
            var sancion = _sancionMapper.CreateSancion(request);

            await _sancionCommand.InsertSancion(sancion);
                        
            var response = _sancionMapper.CreateSancionResponse(sancion);
            return response;           
        }

        public async Task<SancionResponse> DeleteSancion(int sancionId)
        {           
            var sancion = await _sancionQuery.GetSancionById(sancionId);
            if (sancion == null)
            {
                throw new Exception($"No se puede eliminar: La sanción con ID {sancionId} no existe.");
            }

            await _sancionCommand.RemoveSancion(sancionId);

            return _sancionMapper.CreateSancionResponse(sancion);
        }

        public async Task<List<SancionResponse>> GetSancionesPorJugador(int jugadorId)
        {            
            var sanciones = await _sancionQuery.GetSancionesPorJugador(jugadorId);

            
            if (sanciones == null || !sanciones.Any())
            {
                return new List<SancionResponse>();
            }
                        
            var response = sanciones.Select(sancion => _sancionMapper.CreateSancionResponse(sancion)).ToList();
            return response;
        }
    }
}
