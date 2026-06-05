using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Clase
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int CapacidadMax { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public decimal PrecioInscripcion { get; set; }

        // 🔗 Profesor
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

        // 🔗 Cancha (opcional pero recomendada para disponibilidad)
        public int? CanchaId { get; set; }
        public Cancha Cancha { get; set; }

        // 🗓️ Días de la semana (ej: "1,3,5" para Lunes, Miércoles, Viernes)
        public string DiasSemana { get; set; }

        // 🔗 Asistencias (relación real con Cliente)
        public ICollection<Asistencia> Asistencias { get; set; }
    }
}
