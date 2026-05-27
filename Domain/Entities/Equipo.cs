using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Equipo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CantidadMaxJugadores { get; set; }

        public string Descripcion { get; set; }

        public int CantPuntos { get; set; }

        public int PartidosJugados { get; set; }

        public int GolesAFavor { get; set; }

        public int GolesEnContra { get; set; }

        // 🔗 Jugadores
        public ICollection<Jugador> Jugadores { get; set; }

        // 🔗 Competición
        public int CompeticionId { get; set; }
        public Competicion Competicion { get; set; }
    }
}
