using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Profesor : Persona
    {
        public string Certificacion { get; set; }
        public byte[]? CertificadoArchivo { get; set; } // Archivo PDF en BD
        public DateTime? CertificadoFechaInicio { get; set; }
        public DateTime? CertificadoFechaFin { get; set; }

        public string Especialidad { get; set; }


        // Relaciones
        public ICollection<Clase> Clases { get; set; }

        public ICollection<Entrenamiento> Entrenamientos { get; set; }
    }
}
