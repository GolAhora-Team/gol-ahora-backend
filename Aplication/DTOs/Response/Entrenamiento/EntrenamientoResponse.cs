using System;
using System.Collections.Generic;

namespace Aplication.DTOs.Response.Entrenamiento
{
    public class EntrenamientoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public int CupoMaximo { get; set; }
        public int ProfesorId { get; set; }
        public int? CanchaId { get; set; }
        
        // Relaciones
        public EntrenamientoProfesorResponse Profesor { get; set; }
        public List<EntrenamientoClienteResponse> Clientes { get; set; }
    }

    public class EntrenamientoProfesorResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class EntrenamientoClienteResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
