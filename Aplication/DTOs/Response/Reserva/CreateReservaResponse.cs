using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Reserva
{
    public class CreateReservaResponse
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public string Estado { get; set; }

        public int ClienteId { get; set; }

        public int CanchaId { get; set; }
    }
}
