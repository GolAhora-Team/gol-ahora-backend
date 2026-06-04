using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public int UsuarioDestinoId { get; set; }
        [JsonIgnore]
        public Usuario? UsuarioDestino { get; set; }

        public string Tipo { get; set; } = "General";
        
        public bool Leida { get; set; } = false;

        // 🔗 Sistema de invitaciones a equipos
        public bool AccionRequerida { get; set; } = false;
        public int? EquipoId { get; set; }
        public string? EstadoAccion { get; set; } // "Pendiente", "Aceptada", "Rechazada"
        public int? InvitadoPorUsuarioId { get; set; }
    }
}
