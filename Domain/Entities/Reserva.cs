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

        public DateTime HoraInicio { get; set; }

        public DateTime HoraFin { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔗 Cancha
        public int CanchaId { get; set; }
        public Cancha Cancha { get; set; }

        // 🔥 Estado (te recomiendo ENUM)
        public EstadoReserva Estado { get; set; }
    }
}
