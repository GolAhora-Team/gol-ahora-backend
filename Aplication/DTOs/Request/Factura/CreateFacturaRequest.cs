using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Factura
{
    public class CreateFacturaRequest
    {
        public DateTime FechaEmision { get; set; }

        public decimal Total { get; set; }

        public int ClienteId { get; set; }

        public string? Concepto { get; set; }
        public string? Descripcion { get; set; }
    }
}
