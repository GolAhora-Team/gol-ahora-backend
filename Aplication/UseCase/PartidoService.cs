using Aplication.DTOs.Request.Partido;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICompeticion;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.IPartido;
using Aplication.Interfaces.ICancha;
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
        private readonly ICanchaQuery _canchaQuery;
        private readonly ICompeticionCommand _competicionCommand;

        public PartidoService(
            IPartidoMapper mapper, 
            IPartidoCommand command, 
            IPartidoQuery query, 
            ICompeticionQuery competicionQuery,
            ICanchaQuery canchaQuery,
            ICompeticionCommand competicionCommand)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _competicionQuery = competicionQuery;
            _canchaQuery = canchaQuery;
            _competicionCommand = competicionCommand;
        }

        public async Task<PartidoResponse> CargarResultado(int partidoId, CargarResultadoRequest request)
        {            
            var partido = await _query.GetPartidoById(partidoId);
            if (partido == null)
                throw new Exception("El partido no existe.");
           
            partido.GolesLocal = request.GolesLocal;
            partido.GolesVisitante = request.GolesVisitante;
            partido.PenalesLocal = request.PenalesLocal;
            partido.PenalesVisitante = request.PenalesVisitante;
            partido.GanadorId = request.GanadorId;
            
            partido.Estado = EstadoPartido.Finalizado;
            
            await _command.UpdatePartido(partido);
            
            var competicion = await _competicionQuery.GetCompeticionById(partido.CompeticionId);
            if (competicion != null && competicion.Tipo == TipoCompeticion.Torneo && partido.GanadorId.HasValue && partido.Fase != FaseTorneo.Final)
            {
                var partidosFase = await _query.GetPartidosPorFase(partido.CompeticionId, partido.Fase);
                var partidosList = partidosFase.OrderBy(p => p.Id).ToList();
                int index = partidosList.FindIndex(p => p.Id == partido.Id);
                
                if (index != -1)
                {
                    int siblingIndex = index % 2 == 0 ? index + 1 : index - 1;
                    if (siblingIndex >= 0 && siblingIndex < partidosList.Count)
                    {
                        var sibling = partidosList[siblingIndex];
                        if (sibling.Estado == EstadoPartido.Finalizado && sibling.GanadorId.HasValue)
                        {
                            int nextFaseNum = (int)partido.Fase + 1;
                            var nextFaseMatches = await _query.GetPartidosPorFase(partido.CompeticionId, (FaseTorneo)nextFaseNum);
                            
                            bool alreadyCreated = nextFaseMatches.Any(p => 
                                (p.EquipoLocalId == partido.GanadorId.Value && p.EquipoVisitanteId == sibling.GanadorId.Value) ||
                                (p.EquipoLocalId == sibling.GanadorId.Value && p.EquipoVisitanteId == partido.GanadorId.Value));
                                
                            if (!alreadyCreated)
                            {
                                int equipoLocalId = index % 2 == 0 ? partido.GanadorId.Value : sibling.GanadorId.Value;
                                int equipoVisitanteId = index % 2 == 0 ? sibling.GanadorId.Value : partido.GanadorId.Value;
                                
                                var nuevoPartido = new Partido
                                {
                                    CompeticionId = partido.CompeticionId,
                                    EquipoLocalId = equipoLocalId,
                                    EquipoVisitanteId = equipoVisitanteId,
                                    Estado = EstadoPartido.Programado,
                                    Fase = (FaseTorneo)nextFaseNum,
                                    Arbitro = "Por asignar",
                                    Fecha = partido.Fecha.AddDays(7),
                                    Hora = partido.Hora
                                };
                                await _command.InsertPartidos(new List<Partido> { nuevoPartido });
                            }
                        }
                    }
                }
            }
            
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
                await GenerarFixtureTorneo(competicion, equipos);
            }
            else if (competicion.Tipo == TipoCompeticion.Liga)
            {
                await GenerarFixtureLiga(competicion, equipos);
            }
            else
            {
                throw new Exception("El tipo de competición no es válido para generar un fixture automáticamente.");
            }

            competicion.FixtureGenerado = true;
            await _competicionCommand.UpdateCompeticion(competicion);
        }

        private async Task GenerarFixtureLiga(Competicion competicion, List<Equipo> equiposInscritos)
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

            var canchasDisponibles = await _canchaQuery.GetListCancha();
            var canchasAptas = canchasDisponibles.Where(c => c.Tipo == competicion.TipoCancha).ToList();
            var fechaInicio = competicion.FechaInicio ?? DateTime.Today.AddDays(7);

            for (int jornada = 0; jornada < numJornadas; jornada++)
            {
                int canchaIndex = 0;
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

                    var cancha = canchasAptas.Count > 0 ? canchasAptas[canchaIndex % canchasAptas.Count] : null;

                    partidosNuevos.Add(new Partido
                    {
                        CompeticionId = competicion.Id,
                        EquipoLocalId = equipoLocal.Id,
                        EquipoVisitanteId = equipoVisitante.Id,
                        Estado = EstadoPartido.Programado,

                        Jornada = jornada + 1,

                        Fase = 0,
                        Arbitro = "Por asignar",

                        Fecha = fechaInicio.AddDays(jornada * 7),
                        Hora = cancha != null ? cancha.HoraInicio.Add(TimeSpan.FromHours(canchaIndex)) : new TimeSpan(15, 0, 0)
                    });
                    
                    canchaIndex++;
                }
            }

            await _command.InsertPartidos(partidosNuevos);
        }

        private async Task GenerarFixtureTorneo(Competicion competicion, List<Equipo> equiposInscritos)
        {
            var faseActual = DeterminarFaseTorneo(equiposInscritos.Count);

            var canchasDisponibles = await _canchaQuery.GetListCancha();
            var canchasAptas = canchasDisponibles.Where(c => c.Tipo == competicion.TipoCancha).ToList();
            var fechaInicio = competicion.FechaInicio ?? DateTime.Today.AddDays(7);

            var equiposMezclados = equiposInscritos.OrderBy(e => Guid.NewGuid()).ToList();

            var partidosNuevos = new List<Partido>();
            int horasASumar = 0;
            int canchaIndex = 0;

            for (int i = 0; i < equiposMezclados.Count; i += 2)
            {
                var equipoLocal = equiposMezclados[i];
                var equipoVisitante = equiposMezclados[i + 1];
                var cancha = canchasAptas.Count > 0 ? canchasAptas[canchaIndex % canchasAptas.Count] : null;

                var nuevoPartido = new Partido
                {
                    CompeticionId = competicion.Id,
                    EquipoLocalId = equipoLocal.Id,
                    EquipoVisitanteId = equipoVisitante.Id,
                    Estado = EstadoPartido.Programado,
                    Fase = faseActual,
                    Arbitro = "Por asignar",
                    Fecha = fechaInicio,
                    Hora = cancha != null ? cancha.HoraInicio.Add(TimeSpan.FromHours(horasASumar)) : new TimeSpan(14 + horasASumar, 0, 0)
                };

                partidosNuevos.Add(nuevoPartido);
                horasASumar++;
                canchaIndex++;
            }

            await _command.InsertPartidos(partidosNuevos);
        }
    }
}
