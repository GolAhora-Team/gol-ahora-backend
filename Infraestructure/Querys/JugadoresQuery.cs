using Aplication.DTOs;
using Aplication.Interfaces.IJugador;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class JugadoresQuery : IJugadorQuery
    {
        private readonly AppDbContext _context;
        public JugadoresQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<JugadorDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<JugadorDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<JugadorDto?> GetByClienteIdAsync(int clienteId)
        {
            throw new NotImplementedException();
        }
    }
}
