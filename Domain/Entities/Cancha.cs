using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cancha
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool Disponibilidad { get; set; }

        public TipoCancha Tipo { get; set; }

        public int Superficie { get; set; }

        public int Capacidad { get; set; }

        public DateTime HoraInicio { get; set; }

        public DateTime HoraFin { get; set; }

        public int DuracionMax { get; set; }

        public EstadoCancha Estado { get; set; }

        public decimal PrecioPorHora { get; set; }

        // Relaciones
        public ICollection<Reserva> Reservas { get; set; }
        public ICollection<Precio> Precios { get; set; }
    }
}
