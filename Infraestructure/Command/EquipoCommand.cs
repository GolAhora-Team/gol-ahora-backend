using Aplication.Interfaces.IEquipo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class EquipoCommand : IEquipoCommand
    {
        private readonly AppDbContext _context;

        public EquipoCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertEquipo(Equipo equipo)
        {
            _context.Add(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEquipo(int idEquipo)
        {
            var equipo = await _context.Equipos.FindAsync(idEquipo);

            if (equipo != null)
            {
                _context.Remove(equipo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateEquipo(Equipo equipo)
        {
            _context.Update(equipo);
            await _context.SaveChangesAsync();
        }
    }
}
