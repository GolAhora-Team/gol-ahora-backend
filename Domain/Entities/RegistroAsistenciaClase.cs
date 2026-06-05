using System;

namespace Domain.Entities
{
    public class RegistroAsistenciaClase
    {
        public int Id { get; set; }

        public bool Presente { get; set; }

        public DateTime Fecha { get; set; }

        public string? CodigoBarras { get; set; }

        public DateTime? FechaHoraRegistro { get; set; }

        public string? MetodoRegistro { get; set; } // "Manual" o "CodigoBarras"

        // 🔗 Clase
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
