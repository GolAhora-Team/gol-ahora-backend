using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICompeticion;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class CompeticionService : ICompeticionService
    {
        private readonly ICompeticionMapper _mapper;
        private readonly ICompeticionCommand _command;
        private readonly ICompeticionQuery _query;
        private readonly Aplication.Interfaces.INotificacionService _notificacionService;

        public CompeticionService(ICompeticionMapper mapper, ICompeticionCommand command, ICompeticionQuery query, Aplication.Interfaces.INotificacionService notificacionService)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _notificacionService = notificacionService;
        }

        public async Task<CompeticionResponse> CreateCompeticion(CompeticionRequest request)
        {
            if (request.Tipo == TipoCompeticion.Torneo && 
                request.CantidadEquipos != 4 && 
                request.CantidadEquipos != 8 && 
                request.CantidadEquipos != 16)
            {                
                throw new Exception("Los torneos de eliminación directa admiten formato de Semifinal (4 equipos), Cuartos de Final (8 equipos) u Octavos de Final (16 equipos).");
            }

            if (request.Tipo == TipoCompeticion.Liga && 
                request.CantidadEquipos != 4 && 
                request.CantidadEquipos != 8 && 
                request.CantidadEquipos != 20)
            {
                throw new Exception("Por el momento, las ligas deben ser de 4, 8 o 20 equipos.");
            }

            var competicion = _mapper.CreateCompeticion(request);

            competicion.Estado = EstadoCompeticion.EnInscripcion;
            await _command.InsertCompeticion(competicion);

            await _notificacionService.CrearNotificacionGeneral(
                $"Nueva competición abierta: {competicion.Nombre} ({competicion.Tipo}).", 
                "ADMIN,PERSONAL,CLIENTE", 
                "Competicion"
            );

            competicion = await _query.GetCompeticionById(competicion.Id);
            return _mapper.CreateCompeticionResponse(competicion);
        }

        public async Task<CompeticionResponse> DeleteCompeticion(int competicionId)
        {
            var competicion = await _query.GetCompeticionById(competicionId);
            if (competicion == null)
            {
                throw new Exception("Competición no encontrada");
            }

            await _command.RemoveCompeticion(competicion.Id);
            return _mapper.CreateCompeticionResponse(competicion);
        }

        public async Task<List<CompeticionResponse>> GetAll()
        {
            var competicion = await _query.GetListCompeticion();

            return competicion.Select(competicion => _mapper.CreateCompeticionResponse(competicion)).ToList();
        }

        public async Task<CompeticionResponse> GetCompeticionById(int competicionId)
        {
            var competicion = await _query.GetCompeticionById(competicionId);

            if (competicion == null)
                throw new Exception("La competición no existe");

            return _mapper.CreateCompeticionResponse(competicion);
        }

        public async Task<CompeticionResponse> UpdateCompeticion(int competicionId, CompeticionRequest request)
        {
            var competicionOriginal = await _query.GetCompeticionById(competicionId);

            if (competicionOriginal == null)
                throw new Exception("La competición no existe");

            competicionOriginal.Nombre = request.Nombre;            
            competicionOriginal.Descripcion = request.Descripcion;            

            await _command.UpdateCompeticion(competicionOriginal);

            competicionOriginal = await _query.GetCompeticionById(competicionOriginal.Id);
            return _mapper.CreateCompeticionResponse(competicionOriginal);
        }

        public async Task<CompeticionResponse> IniciarCompeticion(int competicionId)
        {
            var competicionOriginal = await _query.GetCompeticionById(competicionId);

            if (competicionOriginal == null)
                throw new Exception("La competición no existe");

            if (!competicionOriginal.FixtureGenerado)
                throw new Exception("El fixture debe estar generado antes de iniciar la competición.");

            competicionOriginal.Estado = EstadoCompeticion.EnJuego;
            await _command.UpdateCompeticion(competicionOriginal);

            return _mapper.CreateCompeticionResponse(competicionOriginal);
        }
    }
}
