using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Clase
{
    public class ClaseCreateResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int capacidadMax { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
