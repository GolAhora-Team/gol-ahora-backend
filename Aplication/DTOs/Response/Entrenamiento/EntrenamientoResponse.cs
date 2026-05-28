using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
