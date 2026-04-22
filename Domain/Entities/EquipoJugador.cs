using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EquipoJugador
    {
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }

        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }
    }
}
