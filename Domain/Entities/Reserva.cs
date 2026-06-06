using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        // 🔗 Cliente
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔗 Partido (Para competiciones)
        public int? PartidoId { get; set; }
        public Partido Partido { get; set; }

        // 🔗 Cancha
        public int CanchaId { get; set; }
        public Cancha Cancha { get; set; }

        // 🔥 Estado (te recomiendo ENUM)
        public EstadoReserva Estado { get; set; }

        // 🔗 Factura de pago (opcional, para reintegros y políticas de cancelación)
        public int? FacturaId { get; set; }
        public Factura Factura { get; set; }
    }
}
