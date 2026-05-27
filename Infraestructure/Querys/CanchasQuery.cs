using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.ICliente;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class CanchasQuery : ICanchaQuery
    {
        private readonly AppDbContext _context;

        public CanchasQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cancha> GetCanchaById(int idCancha)
        {
            return await _context.Canchas.FindAsync(idCancha);
        }

        public async Task<List<Cancha>> GetCanchasActivas()
        {
            return await _context.Canchas.Where(c => c.Disponibilidad == true).ToListAsync();
        }

        public Task<List<Cancha>> GetCanchasDisponibles(DateTime fecha, TimeSpan hora)
        {
            return _context.Canchas.Where(c =>
                c.Disponibilidad == true &&
                !_context.Reservas.Any(r =>
                    r.CanchaId == c.Id &&
                    r.Fecha.Date == fecha.Date &&
                    r.Estado != Domain.Enums.EstadoReserva.Cancelada &&
                    r.HoraInicio <= hora &&
                    r.HoraFin > hora
                )
            ).ToListAsync();
        }

        public Task<List<Cancha>> GetListCancha()
        {
            return _context.Canchas.ToListAsync();
        }

        public Task<bool> CanchaExists(int idCancha)
        {
            return _context.Canchas.AnyAsync(c => c.Id == idCancha);
        }
    }
}
