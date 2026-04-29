using Aplication.Interfaces.IAsistencia;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class AsistneciaCommand : IAsistenciaCommand
    {
        private readonly AppDbContext _context;

        public AsistneciaCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsistencia(Asistencia asistencia)
        {
            await _context.Asistencias.AddAsync(asistencia);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsistencia(Asistencia Asistencia)
        {
             _context.Asistencias.Remove(Asistencia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsistencia(Asistencia asistencia)
        {
            _context.Asistencias.Update(asistencia);
            await _context.SaveChangesAsync();
        }
    }
}
