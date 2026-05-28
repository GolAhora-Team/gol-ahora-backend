using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Partido
{
    public class CargarResultadoRequest
    {
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        
        public int? GanadorId { get; set; }
    }
}
