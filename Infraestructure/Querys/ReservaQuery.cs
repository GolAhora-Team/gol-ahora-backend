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
        TimeSpan horaFin)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.CanchaId == canchaId &&
                r.Fecha.Date == fecha.Date &&
                r.Estado != EstadoReserva.Cancelada &&
                (
                    horaInicio < r.HoraFin &&
                    horaFin > r.HoraInicio
                ));
        }
    }
}
