using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Recibo
    {
        public int Id { get; set; }
        public string NumeroRecibo { get; set; } 
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string Concepto { get; set; }
        public string MetodoPago { get; set; } // Opcional, podría ser MetodoPago enum si quieres vincularlo fuerte
        public string FirmaDigital { get; set; } 

        // 🔗 Reserva (1 a 1)
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }
    }
}
