
using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs.Request.Usuario
{
    public class UploadAptoMedicoRequest
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public string ArchivoBase64 { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }
    }
}
