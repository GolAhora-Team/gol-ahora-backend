using System;
using System.Collections.Generic;

namespace Aplication.DTOs.Request.Asistencia
{
    public class MarcarAsistenciaRequest
    {
        public int ClaseId { get; set; }
        public DateTime Fecha { get; set; }
        public List<int> ClientesPresentesIds { get; set; }
    }
}
