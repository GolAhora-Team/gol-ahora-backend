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
            var usuarios = await _context.Usuarios
                .Include(u => u.Persona)
                .Where(u => 
                    u.Username == identifier || 
                    u.Email == identifier || 
                    u.Persona.Email == identifier || 
                    u.Persona.Dni.ToString() == identifier)
                .ToListAsync();

            return usuarios.FirstOrDefault(u => u.PasswordHash == password);
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
            if (string.IsNullOrWhiteSpace(email)) return null;
            var cleanEmail = email.Trim().ToLower();
            return await _context.Usuarios
                .Include(u => u.Persona)
                .Where(u => (u.Email != null && u.Email.ToLower() == cleanEmail) || 
                            (u.Persona != null && u.Persona.Email != null && u.Persona.Email.ToLower() == cleanEmail))
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetByEmailPassword(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var cleanEmail = email.Trim();
            var usuarios = await _context.Usuarios.Where(u => u.Email.Trim() == cleanEmail).ToListAsync();
            return usuarios.FirstOrDefault(u => u.PasswordHash == password);
        }

        public async Task<Usuario?> GetByResetToken(string token)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpires > DateTime.UtcNow);
        }

        public async Task<List<Usuario>> GetUsersByRoles(List<string> roles)
        {
            var rolesEnum = new List<Domain.Enums.TipoUsuario>();
            foreach (var rol in roles)
            {
                string rolNormalizado = rol.Trim().ToUpper();
                string enumStr = "";
                
                if (rolNormalizado == "ADMIN" || rolNormalizado == "PERSONAL" || rolNormalizado == "ADMINISTRADOR") enumStr = "Administrador";
                else if (rolNormalizado == "PROFE" || rolNormalizado == "PROFESOR") enumStr = "Profesor";
                else if (rolNormalizado == "CLIENTE") enumStr = "Cliente";

                if (!string.IsNullOrEmpty(enumStr) && Enum.TryParse<Domain.Enums.TipoUsuario>(enumStr, out var enumValue))
                {
                    rolesEnum.Add(enumValue);
                }
            }

            return await _context.Usuarios
                .Where(u => rolesEnum.Contains(u.TipoUsuario))
                .ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByPersonaId(int personaId)
        {
            return await _context.Usuarios
                .Include(u => u.Persona)
                .FirstOrDefaultAsync(u => u.Persona.Id == personaId);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.Include(s => s.Persona).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _context.Usuarios
                .Include(u => u.Persona)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.Trim().ToLower());
        }
    }
}
