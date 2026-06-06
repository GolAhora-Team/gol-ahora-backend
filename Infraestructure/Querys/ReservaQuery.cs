using Aplication.Interfaces.IReserva;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ReservaQuery : IReservaQuery
    {
        private readonly AppDbContext _context;
        public ReservaQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Domain.Entities.Reserva>> GetAllReservas()
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Cliente)
                .Include(r => r.Partido).ThenInclude(p => p.Competicion)
                .Include(r => r.Partido).ThenInclude(p => p.EquipoLocal)
                .Include(r => r.Partido).ThenInclude(p => p.EquipoVisitante)
                .ToListAsync();
        }
        public async Task<Domain.Entities.Reserva?> GetReservaById(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Cliente)
                .Include(r => r.Partido).ThenInclude(p => p.Competicion)
                .Include(r => r.Partido).ThenInclude(p => p.EquipoLocal)
                .Include(r => r.Partido).ThenInclude(p => p.EquipoVisitante)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<bool> ReservaExists(int id)
        {
            return await _context.Reservas.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> ExisteReservaEnHorario(
        int canchaId,
        DateTime fecha,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int? excludeReservaId = null)
        {
            bool conflictoReserva = await _context.Reservas.AnyAsync(r =>
                r.CanchaId == canchaId &&
                r.Fecha.Date == fecha.Date &&
                r.Estado != EstadoReserva.Cancelada &&
                (!excludeReservaId.HasValue || r.Id != excludeReservaId.Value) &&
                (
                    horaInicio < r.HoraFin &&
                    horaFin > r.HoraInicio
                ));

            if (conflictoReserva) return true;

            string diaAbreviado = GetDiaAbreviado(fecha.DayOfWeek);

            bool conflictoClase = await _context.Clases.AnyAsync(c =>
                c.CanchaId == canchaId &&
                c.DiasSemana != null && c.DiasSemana.Contains(diaAbreviado) &&
                (
                    horaInicio < c.HoraFin &&
                    horaFin > c.HoraInicio
                ));

            if (conflictoClase) return true;

            bool conflictoEntrenamiento = await _context.Entrenamientos.AnyAsync(e =>
                e.CanchaId == canchaId &&
                e.DiasSemana != null && e.DiasSemana.Contains(diaAbreviado) &&
                (
                    horaInicio < e.HoraFin &&
                    horaFin > e.HoraInicio
                ));

            return conflictoEntrenamiento;
        }

        public async Task<string?> ObtenerConflictoParaNuevaActividad(
            int canchaId,
            string diasSemana,
            TimeSpan horaInicio,
            TimeSpan horaFin,
            string tipoActividad,
            int? excludeActividadId = null)
        {
            if (string.IsNullOrEmpty(diasSemana)) return null;

            var diasList = diasSemana.Split(',').Select(d => d.Trim()).ToList();
            var today = DateTime.Today;

            // 1. Validar contra Reservas en los próximos 30 días
            for (int i = 0; i <= 30; i++)
            {
                var fecha = today.AddDays(i);
                var diaAbrev = GetDiaAbreviado(fecha.DayOfWeek);
                if (diasList.Contains(diaAbrev))
                {
                    var reservaConflicto = await _context.Reservas
                        .Include(r => r.Cliente)
                        .FirstOrDefaultAsync(r =>
                            r.CanchaId == canchaId &&
                            r.Fecha.Date == fecha.Date &&
                            r.Estado != EstadoReserva.Cancelada &&
                            horaInicio < r.HoraFin &&
                            horaFin > r.HoraInicio);

                    if (reservaConflicto != null)
                    {
                        return $"Ya existe una reserva previa el día {fecha:dd/MM/yyyy} en el horario {reservaConflicto.HoraInicio.ToString(@"hh\:mm")} - {reservaConflicto.HoraFin.ToString(@"hh\:mm")}. No hay clase ese día.";
                    }
                }
            }

            // 2. Validar contra otras Clases
            foreach (var dia in diasList)
            {
                var claseConflicto = await _context.Clases
                    .FirstOrDefaultAsync(c =>
                        c.CanchaId == canchaId &&
                        c.DiasSemana != null && c.DiasSemana.Contains(dia) &&
                        (!excludeActividadId.HasValue || tipoActividad != "CLASE" || c.Id != excludeActividadId.Value) &&
                        horaInicio < c.HoraFin &&
                        horaFin > c.HoraInicio);

                if (claseConflicto != null)
                {
                    return $"Ya existe la clase '{claseConflicto.Nombre}' dictada los días {claseConflicto.DiasSemana} en el horario {claseConflicto.HoraInicio.ToString(@"hh\:mm")} - {claseConflicto.HoraFin.ToString(@"hh\:mm")}.";
                }
            }

            // 3. Validar contra otros Entrenamientos
            foreach (var dia in diasList)
            {
                var entrenamientoConflicto = await _context.Entrenamientos
                    .FirstOrDefaultAsync(e =>
                        e.CanchaId == canchaId &&
                        e.DiasSemana != null && e.DiasSemana.Contains(dia) &&
                        (!excludeActividadId.HasValue || tipoActividad != "ENTRENAMIENTO" || e.Id != excludeActividadId.Value) &&
                        horaInicio < e.HoraFin &&
                        horaFin > e.HoraInicio);

                if (entrenamientoConflicto != null)
                {
                    return $"Ya existe el entrenamiento '{entrenamientoConflicto.Nombre}' dictado los días {entrenamientoConflicto.DiasSemana} en el horario {entrenamientoConflicto.HoraInicio.ToString(@"hh\:mm")} - {entrenamientoConflicto.HoraFin.ToString(@"hh\:mm")}.";
                }
            }

            return null;
        }

        private string GetDiaAbreviado(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "Lun",
                DayOfWeek.Tuesday => "Mar",
                DayOfWeek.Wednesday => "Mié",
                DayOfWeek.Thursday => "Jue",
                DayOfWeek.Friday => "Vie",
                DayOfWeek.Saturday => "Sáb",
                DayOfWeek.Sunday => "Dom",
                _ => ""
            };
        }
    }
}

