using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Factura
{
    public class FacturaResponse
    {
        public int Id { get; set; }

        public DateTime FechaEmision { get; set; }

        public decimal Total { get; set; }

        public int ClienteId { get; set; }
    }
}
