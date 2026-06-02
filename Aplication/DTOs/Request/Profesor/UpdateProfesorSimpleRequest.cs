using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Profesor
{
    public class UpdateProfesorSimpleRequest
    {
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string ContactoEmergencia { get; set; }
        public string Email { get; set; }
        public string? CertificadoBase64 { get; set; }
        public DateTime? CertificadoFechaInicio { get; set; }
        public DateTime? CertificadoFechaFin { get; set; }
    }
}
