using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Clase
{
    public class ClaseResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int CapacidadMax { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public decimal PrecioInscripcion { get; set; }

        // Profesor
        public ProfesorShortResponse Profesor { get; set; }
        public List<ClaseAlumnos> Alumnos { get; set; }
    }

    public class ProfesorShortResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string especialidad { get; set; }
    }

    public class ClaseAlumnos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
