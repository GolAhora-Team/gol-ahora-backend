using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Equipo
{
    public class CreateEquipoRequest
    {       
        public string Nombre { get; set; }

        public int CantidadMaxJugadores { get; set; }

        public string Descripcion { get; set; }

        public int? CompeticionId { get; set; }
    }
}
