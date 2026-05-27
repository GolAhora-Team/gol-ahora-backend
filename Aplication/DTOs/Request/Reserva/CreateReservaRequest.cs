using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Reserva
{
    public class CreateReservaRequest
    {
        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public int ClienteId { get; set; }

        public int CanchaId { get; set; }
    }
}
