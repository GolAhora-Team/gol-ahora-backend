using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public TipoUsuario TipoUsuario { get; set; }

        // Recuperación de Contraseña
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        // 🔗 relación 1:1
        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
    }
}
