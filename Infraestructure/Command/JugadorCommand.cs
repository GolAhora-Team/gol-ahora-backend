using Aplication.DTOs;
using Aplication.Interfaces.IJugador;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class JugadoresCommand : IJugadorCommand
    {
        private readonly AppDbContext _context;
        public JugadoresCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<JugadorDto> CreateAsync(JugadorDto jugadorDto)
        {
            throw new NotImplementedException();
        }
        public async Task<JugadorDto?> UpdateAsync(int id, JugadorDto jugadorDto)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
