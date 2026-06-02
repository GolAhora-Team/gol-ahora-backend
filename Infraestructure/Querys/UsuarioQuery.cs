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

        public async Task<Usuario?> GetByIdentifierAndPassword(string identifier, string password)
        {
            return await _context.Usuarios
                .Include(u => u.Persona)
                .Where(u => 
                    (u.Username == identifier || 
                     u.Email == identifier || 
                     u.Persona.Email == identifier || 
                     u.Persona.Dni.ToString() == identifier) 
                    && u.PasswordHash == password)
                .FirstOrDefaultAsync();
        }

        public async Task<List<string>> CheckUniqueness(int dni, string email, string username)
        {
            var takenFields = new List<string>();

            bool dniTaken = await _context.Personas.AnyAsync(p => p.Dni == dni);
            if (dniTaken) takenFields.Add("DNI");

            bool emailTaken = await _context.Usuarios.AnyAsync(u => u.Email == email) || 
                              await _context.Personas.AnyAsync(p => p.Email == email);
            if (emailTaken) takenFields.Add("Email");

            bool usernameTaken = await _context.Usuarios.AnyAsync(u => u.Username == username);
            if (usernameTaken) takenFields.Add("Username");

            return takenFields;
        }

        public async Task<Usuario?> GetByEmail(string email)
        {
            return await _context.Usuarios.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.Include(s => s.Persona).FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
