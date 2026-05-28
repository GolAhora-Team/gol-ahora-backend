using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ReservaDto
    {
        public int ClienteId { get; set; }

        public int CanchaId { get; set; }

        public DateTime FechaReserva { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }
    }
}
