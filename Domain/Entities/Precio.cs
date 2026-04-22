using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Precio
    {
        public int Id { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaVigenciaDesde { get; set; }

        public DateTime? FechaVigenciaHasta { get; set; }

        // Clases
        public int CanchaId { get; set; }
        public Cancha Cancha { get; set; }

        public int? DescuentoId { get; set; }
        public Descuento Descuento { get; set; }
    }
}
