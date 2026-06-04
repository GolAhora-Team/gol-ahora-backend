using System;

namespace Domain.Entities
{
    public class CertificadoProfesor
    {
        public int Id { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

        public string CertificadoUrl { get; set; }
        public string CertificadoNombreArchivo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}
