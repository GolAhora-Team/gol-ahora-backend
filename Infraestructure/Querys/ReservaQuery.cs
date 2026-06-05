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
            return await _context.Reservas.Include(r => r.Cancha).Include(r => r.Cliente).ToListAsync();
        }
        public async Task<Domain.Entities.Reserva?> GetReservaById(int id)
        {
            return await _context.Reservas.Include(r => r.Cancha).Include(r => r.Cliente).FirstOrDefaultAsync(r => r.Id == id);
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
