using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Factura
    {
        public int Id { get; set; }

        public DateTime FechaEmision { get; set; }

        public decimal Total { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔗 Relación con pagos
        public ICollection<Pago> Pagos { get; set; }
    }
}
