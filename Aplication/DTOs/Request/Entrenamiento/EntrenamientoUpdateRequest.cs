using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Entrenamiento
{
    public class EntrenamientoUpdateRequest
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public int CupoMaximo { get; set; }
        public int? ProfesorId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string DiasSemana { get; set; }
        public int? CanchaId { get; set; }
        public decimal PrecioInscripcion { get; set; }
    }
}
