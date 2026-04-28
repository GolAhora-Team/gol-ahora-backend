using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Descuento
{
    public class CreateDescuentoRequest
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Porcentaje { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
