using Aplication.Interfaces.IClases;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ClaseCommand: IClaseCommand
    {
        private readonly AppDbContext _context;
        public ClaseCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task InsertClase(Clase clase)
        {
            await _context.Clases.AddAsync(clase);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveClase(Clase idClase)
        {
            _context.Clases.Remove(idClase);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateClase(Clase clase)
        {
            _context.Clases.Update(clase);
            await _context.SaveChangesAsync();
        }
    }
}
