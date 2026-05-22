using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Pago
{
    public class CreatePagoRequest
    {
        public DateTime FechaPago { get; set; }

        public decimal Monto { get; set; }

        public MetodoPago Metodo { get; set; }

        public EstadoPago Estado { get; set; }

        public int FacturaId { get; set; }
    }
}
