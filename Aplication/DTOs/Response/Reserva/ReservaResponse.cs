using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Reserva
{
    public class ReservaResponse
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public string Estado { get; set; }

        public ClienteShort Cliente { get; set; }

        public CanchaShort Cancha { get; set; }
    }

    public class ClienteShort
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string apellido { get; set; }
    }

    public class CanchaShort
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Capacidad { get; set; }
    }
}
