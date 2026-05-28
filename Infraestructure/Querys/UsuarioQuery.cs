using Aplication.Interfaces.IUsuario;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class UsuarioQuery : IUsuarioQuery
    {
        private readonly AppDbContext _context;

        public UsuarioQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAll()
        {
            return await _context.Usuarios.Include(s => s.Persona).ToListAsync();
        }

        public async Task<Usuario?> GetByEmailPassword(string email, string password)
        {
            return await _context.Usuarios.Where(u => u.Email == email && u.PasswordHash == password).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.Include(s => s.Persona).FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
