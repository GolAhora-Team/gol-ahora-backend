using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Factura
{
    public class UpdateFacturaRequest
    {
        public DateTime FechaEmision { get; set; }

        public decimal Total { get; set; }

        public int ClienteId { get; set; }
    }
}
