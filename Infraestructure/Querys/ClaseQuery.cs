using Aplication.Interfaces.IClases;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ClaseQuery: IClaseQuery
    {
        private readonly AppDbContext _context;
    
            public ClaseQuery(AppDbContext context)
            {
                _context = context;
            }
    
            public async Task<bool> ClaseExists(int id)
            {
                return await _context.Clases.AnyAsync(c => c.Id == id);
            }
    
            public async Task<List<Clase>> GetAllClases()
            {
                return await _context.Clases.Include(c => c.Profesor)
                                            .Include(c => c.Cancha)
                                            .Include(c => c.Asistencias).ThenInclude(a => a.Cliente)
                                            .ToListAsync();
            }
    
            public async Task<Clase?> GetClaseById(int id)
            {
                return await _context.Clases.Include(c => c.Profesor)
                                            .Include(c => c.Cancha)
                                            .Include(c => c.Asistencias).ThenInclude(a => a.Cliente)
                                            .FirstOrDefaultAsync(c => c.Id == id);
            }
    }
}
