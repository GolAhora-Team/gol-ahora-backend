using System;

namespace Aplication.DTOs
{
    public class CertificadoProfesorDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Estado { get; set; }
    }
}
