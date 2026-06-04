using Microsoft.AspNetCore.Http;
using System;

namespace Aplication.DTOs.Request.Usuario
{
    public class CreateUsuarioProfesorFormRequest
    {
        // Campos de Usuario
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        // Campos de Profesor
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
        public string Certificacion { get; set; }
        public string Especialidad { get; set; }
        
        // Campos de Certificado (Archivo)
        public string? CertificadoBase64 { get; set; }
        public DateTime? CertificadoFechaInicio { get; set; }
        public DateTime? CertificadoFechaVencimiento { get; set; }
    }
}
