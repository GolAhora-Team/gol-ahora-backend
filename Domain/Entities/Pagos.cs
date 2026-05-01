using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pago
    {
        public int Id { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal Monto { get; set; }

        public MetodoPago Metodo { get; set; }

        public EstadoPago Estado { get; set; }

        // 🔗 Factura
        public int FacturaId { get; set; }
        public Factura Factura { get; set; }
    }
}
