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

        public string ColorPrimario { get; set; } = "#ffffff";
        public string ColorSecundario { get; set; } = "#ffffff";

        public string? TipoCancha { get; set; }
        public string? FormacionDefecto { get; set; }

        public int CantPuntos { get; set; }

        public int PartidosJugados { get; set; }

        public int GolesAFavor { get; set; }

        public int GolesEnContra { get; set; }

        // 🔗 Jugadores (muchos a muchos)
        public ICollection<EquipoJugador> Jugadores { get; set; }

        // 🔗 Cliente creador
        public int? CreadoPorClienteId { get; set; }
        public Cliente? CreadoPorCliente { get; set; }

        // 🔗 Competición
        public int? CompeticionId { get; set; }
        public Competicion? Competicion { get; set; }
    }
}
