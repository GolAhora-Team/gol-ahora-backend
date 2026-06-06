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
        private readonly Aplication.Interfaces.IReserva.IReservaCommand _reservaCommand;
        private readonly Aplication.Interfaces.IReserva.IReservaQuery _reservaQuery;

        public PartidoService(
            IPartidoMapper mapper, 
            IPartidoCommand command, 
            IPartidoQuery query, 
            ICompeticionQuery competicionQuery,
            ICanchaQuery canchaQuery,
            ICompeticionCommand competicionCommand,
            Aplication.Interfaces.IReserva.IReservaCommand reservaCommand,
            Aplication.Interfaces.IReserva.IReservaQuery reservaQuery)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _competicionQuery = competicionQuery;
            _canchaQuery = canchaQuery;
            _competicionCommand = competicionCommand;
            _reservaCommand = reservaCommand;
            _reservaQuery = reservaQuery;
        }

        public async Task<PartidoResponse> CargarResultado(int partidoId, CargarResultadoRequest request)
        {            
            var partido = await _query.GetPartidoById(partidoId);
            if (partido == null)
                throw new Exception("El partido no existe.");
            int? oldGanadorId = partido.GanadorId;

            partido.GolesLocal = request.GolesLocal;
            partido.GolesVisitante = request.GolesVisitante;
            partido.PenalesLocal = request.PenalesLocal;
            partido.PenalesVisitante = request.PenalesVisitante;
            partido.GanadorId = request.GanadorId;
            
            partido.Estado = request.GanadorId.HasValue ? EstadoPartido.Finalizado : EstadoPartido.Programado;
            
            await _command.UpdatePartido(partido);
            
            var competicion = await _competicionQuery.GetCompeticionById(partido.CompeticionId);
            if (competicion != null && competicion.Tipo == TipoCompeticion.Torneo && partido.Fase != FaseTorneo.Final)
            {
                var partidosFase = await _query.GetPartidosPorFase(partido.CompeticionId, partido.Fase);
                var partidosList = partidosFase.OrderBy(p => p.Id).ToList();
                int index = partidosList.FindIndex(p => p.Id == partido.Id);
                
                if (index != -1)
                {
                    int nextFaseNum = (int)partido.Fase + 1;
                    var nextFaseMatches = await _query.GetPartidosPorFase(partido.CompeticionId, (FaseTorneo)nextFaseNum);
                    var nextFaseMatchesList = nextFaseMatches.OrderBy(p => p.Id).ToList();
                    
                    if (nextFaseMatchesList.Count > index / 2)
                    {
                        var nextMatchResponse = nextFaseMatchesList[index / 2];
                        var nextMatch = await _query.GetPartidoById(nextMatchResponse.Id);
                        
                        if (index % 2 == 0)
                        {
                            nextMatch.EquipoLocalId = request.GanadorId;
                        }
                        else
                        {
                            nextMatch.EquipoVisitanteId = request.GanadorId;
                        }
                        await _command.UpdatePartido(nextMatch);
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

        private async Task<(Cancha, DateTime, TimeSpan)> AsignarHorarioYCancha(DateTime fechaInicial, int duracionMinutos, List<Cancha> canchasAptas)
        {
            DateTime fechaActual = fechaInicial;

            while (true) // Buscar hasta encontrar
            {
                foreach (var cancha in canchasAptas)
                {
                    TimeSpan horaActual = cancha.HoraInicio;
                    while (horaActual + TimeSpan.FromMinutes(duracionMinutos) <= cancha.HoraFin)
                    {
                        bool ocupado = await _reservaQuery.ExisteReservaEnHorario(
                            cancha.Id,
                            fechaActual,
                            horaActual,
                            horaActual + TimeSpan.FromMinutes(duracionMinutos));

                        if (!ocupado)
                        {
                            return (cancha, fechaActual, horaActual);
                        }
                        horaActual = horaActual.Add(TimeSpan.FromMinutes(duracionMinutos));
                    }
                }
                fechaActual = fechaActual.AddDays(1);
            }
        }

        private async Task GenerarFixtureLiga(Competicion competicion, List<Equipo> equiposInscritos)
        {
            if (equiposInscritos.Count % 2 != 0)
            {
                throw new Exception("No se puede generar el fixture. La liga debe tener un número par de equipos inscritos.");
            }

            // Remove existing matches to prevent duplication if called multiple times
            await _command.DeletePartidosPorCompeticion(competicion.Id);

            var equipos = equiposInscritos.OrderBy(e => Guid.NewGuid()).ToList();

            int numEquipos = equipos.Count;
            int numJornadas = numEquipos - 1;
            int partidosPorJornada = numEquipos / 2;

            var canchasDisponibles = await _canchaQuery.GetListCancha();
            var canchasAptas = canchasDisponibles.Where(c => c.Tipo == competicion.TipoCancha && c.Disponibilidad && c.Estado == EstadoCancha.Disponible).ToList();
            if (!canchasAptas.Any()) throw new Exception("No hay canchas disponibles para el tipo de cancha seleccionado.");

            int duracionMinutos = competicion.TipoCancha == TipoCancha.Futbol11 ? 90 : 60;
            var fechaInicio = competicion.FechaInicio ?? DateTime.Today.AddDays(7);

            for (int jornada = 0; jornada < numJornadas; jornada++)
            {
                for (int i = 0; i < partidosPorJornada; i++)
                {
                    int localIndex = (jornada + i) % (numEquipos - 1);
                    int visitanteIndex = (numEquipos - 1 - i + jornada) % (numEquipos - 1);

                    if (i == 0) visitanteIndex = numEquipos - 1;

                    var equipoLocal = equipos[localIndex];
                    var equipoVisitante = equipos[visitanteIndex];

                    if (i == 0 && jornada % 2 != 0)
                    {
                        var temp = equipoLocal;
                        equipoLocal = equipoVisitante;
                        equipoVisitante = temp;
                    }

                    DateTime fechaJornada = fechaInicio.AddDays(jornada * 7);
                    var asignacion = await AsignarHorarioYCancha(fechaJornada, duracionMinutos, canchasAptas);

                    var nuevoPartido = new Partido
                    {
                        CompeticionId = competicion.Id,
                        EquipoLocalId = equipoLocal.Id,
                        EquipoVisitanteId = equipoVisitante.Id,
                        Estado = EstadoPartido.Programado,
                        Jornada = jornada + 1,
                        Fase = 0,
                        Arbitro = "Por asignar",
                        CanchaId = asignacion.Item1.Id,
                        Fecha = asignacion.Item2,
                        Hora = asignacion.Item3
                    };

                    await _command.InsertPartidos(new List<Partido> { nuevoPartido });

                    var reserva = new Reserva
                    {
                        Fecha = nuevoPartido.Fecha,
                        HoraInicio = nuevoPartido.Hora,
                        HoraFin = nuevoPartido.Hora.Add(TimeSpan.FromMinutes(duracionMinutos)),
                        CanchaId = asignacion.Item1.Id,
                        Estado = EstadoReserva.Confirmada,
                        PartidoId = nuevoPartido.Id,
                        ClienteId = null
                    };

                    await _reservaCommand.InsertReserva(reserva);
                }
            }
        }

        private async Task GenerarFixtureTorneo(Competicion competicion, List<Equipo> equiposInscritos)
        {
            var faseInicial = DeterminarFaseTorneo(equiposInscritos.Count);

            // Remove existing matches to prevent duplication if called multiple times
            await _command.DeletePartidosPorCompeticion(competicion.Id);

            var canchasDisponibles = await _canchaQuery.GetListCancha();
            var canchasAptas = canchasDisponibles.Where(c => c.Tipo == competicion.TipoCancha && c.Disponibilidad && c.Estado == EstadoCancha.Disponible).ToList();
            if (!canchasAptas.Any()) throw new Exception("No hay canchas disponibles para el tipo de cancha seleccionado.");

            int duracionMinutos = competicion.TipoCancha == TipoCancha.Futbol11 ? 90 : 60;
            var fechaInicio = competicion.FechaInicio ?? DateTime.Today.AddDays(7);
            var equiposMezclados = equiposInscritos.OrderBy(e => Guid.NewGuid()).ToList();

            int faseMax = (int)FaseTorneo.Final;
            int faseNum = (int)faseInicial;

            while (faseNum <= faseMax)
            {
                int cantidadPartidosEnFase = (int)Math.Pow(2, faseMax - faseNum);

                for (int i = 0; i < cantidadPartidosEnFase; i++)
                {
                    var asignacion = await AsignarHorarioYCancha(fechaInicio, duracionMinutos, canchasAptas);

                    var nuevoPartido = new Partido
                    {
                        CompeticionId = competicion.Id,
                        Estado = EstadoPartido.Programado,
                        Fase = (FaseTorneo)faseNum,
                        Arbitro = "Por asignar",
                        CanchaId = asignacion.Item1.Id,
                        Fecha = asignacion.Item2,
                        Hora = asignacion.Item3
                    };

                    // Asignar equipos solo a la primera fase
                    if (faseNum == (int)faseInicial)
                    {
                        nuevoPartido.EquipoLocalId = equiposMezclados[i * 2].Id;
                        nuevoPartido.EquipoVisitanteId = equiposMezclados[i * 2 + 1].Id;
                    }
                    else
                    {
                        nuevoPartido.EquipoLocalId = null;
                        nuevoPartido.EquipoVisitanteId = null;
                    }

                    await _command.InsertPartidos(new List<Partido> { nuevoPartido });

                    var reserva = new Reserva
                    {
                        Fecha = nuevoPartido.Fecha,
                        HoraInicio = nuevoPartido.Hora,
                        HoraFin = nuevoPartido.Hora.Add(TimeSpan.FromMinutes(duracionMinutos)),
                        CanchaId = asignacion.Item1.Id,
                        Estado = EstadoReserva.Confirmada,
                        PartidoId = nuevoPartido.Id,
                        ClienteId = null
                    };

                    await _reservaCommand.InsertReserva(reserva);
                }

                // La próxima fase se juega 7 días después de la fecha de inicio actual de esta fase.
                fechaInicio = fechaInicio.AddDays(7);
                faseNum++;
            }
        }
    }
}
