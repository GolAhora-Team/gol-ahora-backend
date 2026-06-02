using System;

namespace Aplication.DTOs.Response.Asistencia
{
    public class AsistenciaResponse
    {
        public int Id { get; set; }
        public bool Presente { get; set; }
        public DateTime Fecha { get; set; }
        public int ClaseId { get; set; }
        public int ClienteId { get; set; }
    }
}
