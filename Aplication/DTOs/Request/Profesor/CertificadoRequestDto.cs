using System;

namespace Aplication.DTOs.Request.Profesor
{
    public class CertificadoRequestDto
    {
        public int? Id { get; set; } // Si tiene ID, es uno existente que se está editando o manteniendo
        public string? CertificadoBase64 { get; set; } // Archivo nuevo
        public DateTime? CertificadoFechaInicio { get; set; }
        public DateTime? CertificadoFechaVencimiento { get; set; }
        public bool Eliminar { get; set; } // Flag para eliminar el certificado existente
    }
}
