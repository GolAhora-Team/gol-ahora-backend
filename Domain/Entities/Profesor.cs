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
        public string Especialidad { get; set; }

        public ICollection<CertificadoProfesor> Certificados { get; set; }

        // Relaciones
        public ICollection<Clase> Clases { get; set; }

        public ICollection<Entrenamiento> Entrenamientos { get; set; }
    }
}
