using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Profesor
{
    public class ProfesorResponse
    {
        public int Id { get; set; }
        public string Certificacion { get; set; }
        public string Especialidad { get; set; }
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string ContactoEmergencia { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? CertificadoFechaInicio { get; set; }
        public DateTime? CertificadoFechaVencimiento { get; set; }
        public bool TieneCertificado { get; set; }
        
        public string? CertificadoUrl { get; set; }
        public string? CertificadoNombreArchivo { get; set; }
        public string? CertificadoEstado { get; set; }
    }
}
