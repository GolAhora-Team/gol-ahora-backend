using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Sancion
{
    public class SancionRequest
    {       
        public DateTime Fecha { get; set; }        

        public TipoTarjeta Tarjeta { get; set; }
       
        public int JugadorId { get; set; }        
    }
}
