using System;

namespace Domain.Entities
{
    public class AsistenciaEntrenamiento
    {
        public int Id { get; set; }

        public bool Presente { get; set; }

        public DateTime Fecha { get; set; }

        public string? CodigoBarras { get; set; }

        public DateTime? FechaHoraRegistro { get; set; }

        public string? MetodoRegistro { get; set; } // "Manual" o "CodigoBarras"

        // 🔗 Entrenamiento
        public int EntrenamientoId { get; set; }
        public Entrenamiento Entrenamiento { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
