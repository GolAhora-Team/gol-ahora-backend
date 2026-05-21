using Aplication.DTOs.Request.Partido;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICompeticion;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.IPartido;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class PartidoService : IPartidoService
    {
        private readonly IPartidoMapper _mapper;
        private readonly IPartidoCommand _command;
        private readonly IPartidoQuery _query;
        private readonly ICompeticionQuery _competicionQuery;

        public PartidoService(IPartidoMapper mapper, IPartidoCommand command, IPartidoQuery query, ICompeticionQuery competicionQuery)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _competicionQuery = competicionQuery;
        }

        public async Task<PartidoResponse> CargarResultado(int partidoId, CargarResultadoRequest request)
        {            
            var partido = await _query.GetPartidoById(partidoId);
            if (partido == null)
                throw new Exception("El partido no existe.");
           
            partido.GolesLocal = request.GolesLocal;
            partido.GolesVisitante = request.GolesVisitante;
            partido.GanadorId = request.GanadorId;
            
            partido.Estado = EstadoPartido.Finalizado;
            
            await _command.UpdatePartido(partido);
            
            return _mapper.CreatePartidoResponse(partido);
        }

        

        public async Task<PartidoResponse> GetPartidoById(int partidoId)
        {
            var partido = await _query.GetPartidoById(partidoId);

            if (partido == null)
                throw new Exception("El partido solicitado no existe.");
                        
            return _mapper.CreatePartidoResponse(partido);
        }

        public async Task<List<PartidoResponse>> GetPartidosPorCompeticion(int competicionId)
        {
            return await _query.GetPartidosPorCompeticion(competicionId);
        }

        public async Task<List<PartidoResponse>> GetPartidosPorFase(int competicionId, FaseTorneo fase)
        {
            return await _query.GetPartidosPorFase(competicionId, fase);
        }


        private FaseTorneo DeterminarFaseTorneo(int cantidadEquipos)
        {            
            return cantidadEquipos switch
            {
                16 => FaseTorneo.OctavosDeFinal,
                8 => FaseTorneo.CuartosDeFinal,
                4 => FaseTorneo.Semifinal,
                2 => FaseTorneo.Final,
                _ => throw new Exception($"Cantidad de equipos inválida ({cantidadEquipos}). Un torneo debe tener 2, 4, 8 o 16 equipos.")
            };
        }

        public async Task GenerarFixture(int competicionId)
        {            
            var competicion = await _competicionQuery.GetCompeticionConEquipos(competicionId);
                        
            if (competicion == null)
            {
                throw new Exception("La competición especificada no existe.");
            }

            var equipos = competicion.Equipos?.ToList();

            if (equipos == null || equipos.Count == 0)
            {
                throw new Exception("No se puede generar el fixture porque no hay equipos inscritos en esta competición.");
            }
                        
            if (competicion.Tipo == TipoCompeticion.Torneo) 
            {
                await GenerarFixtureTorneo(competicionId, equipos);
            }
            else if (competicion.Tipo == TipoCompeticion.Liga)
            {
                await GenerarFixtureLiga(competicionId, equipos);
            }
            else
            {
                throw new Exception("El tipo de competición no es válido para generar un fixture automáticamente.");
            }
        }

        private async Task GenerarFixtureLiga(int competicionId, List<Equipo> equiposInscritos)
        {
            if (equiposInscritos.Count % 2 != 0)
            {
                throw new Exception("No se puede generar el fixture. La liga debe tener un número par de equipos inscritos.");
            }

            var equipos = equiposInscritos.OrderBy(e => Guid.NewGuid()).ToList();

            int numEquipos = equipos.Count;
            int numJornadas = numEquipos - 1;
            int partidosPorJornada = numEquipos / 2;

            var partidosNuevos = new List<Partido>();

            for (int jornada = 0; jornada < numJornadas; jornada++)
            {
                for (int i = 0; i < partidosPorJornada; i++)
                {
                    int localIndex = (jornada + i) % (numEquipos - 1);
                    int visitanteIndex = (numEquipos - 1 - i + jornada) % (numEquipos - 1);

                    if (i == 0)
                    {
                        visitanteIndex = numEquipos - 1;
                    }

                    var equipoLocal = equipos[localIndex];
                    var equipoVisitante = equipos[visitanteIndex];

                    if (i == 0 && jornada % 2 != 0)
                    {
                        var temp = equipoLocal;
                        equipoLocal = equipoVisitante;
                        equipoVisitante = temp;
                    }

                    partidosNuevos.Add(new Partido
                    {
                        CompeticionId = competicionId,
                        EquipoLocalId = equipoLocal.Id,
                        EquipoVisitanteId = equipoVisitante.Id,
                        Estado = EstadoPartido.Programado,

                        Jornada = jornada + 1,

                        Fase = 0,

                        Fecha = DateTime.Today.AddDays((jornada + 1) * 7),
                        Hora = new TimeSpan(15, 0, 0)
                    });
                }
            }

            await _command.InsertPartidos(partidosNuevos);
        }

        private async Task GenerarFixtureTorneo(int competicionId, List<Equipo> equiposInscritos)
        {
            var faseActual = DeterminarFaseTorneo(equiposInscritos.Count);

            var equiposMezclados = equiposInscritos.OrderBy(e => Guid.NewGuid()).ToList();

            var partidosNuevos = new List<Partido>();
            int horasASumar = 0;

            for (int i = 0; i < equiposMezclados.Count; i += 2)
            {
                var equipoLocal = equiposMezclados[i];
                var equipoVisitante = equiposMezclados[i + 1];

                var nuevoPartido = new Partido
                {
                    CompeticionId = competicionId,
                    EquipoLocalId = equipoLocal.Id,
                    EquipoVisitanteId = equipoVisitante.Id,
                    Estado = EstadoPartido.Programado,
                    Fase = faseActual,
                    Fecha = DateTime.Today.AddDays(7),
                    Hora = new TimeSpan(14 + horasASumar, 0, 0)
                };

                partidosNuevos.Add(nuevoPartido);
                horasASumar++;
            }

            await _command.InsertPartidos(partidosNuevos);
        }
    }
}
