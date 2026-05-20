using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Clase
{
    public class ClaseCreateRequest
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int CapacidadMax { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public decimal PrecioInscripcion { get; set; }

        public int ProfesorId { get; set; }
    }
}
