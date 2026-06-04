using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Competicion
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public TipoCompeticion Tipo { get; set; }

        public string Descripcion { get; set; }

        public int CantidadEquipos { get; set; }

        public EstadoCompeticion Estado { get; set; }

        // Nuevos campos
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public TipoCancha TipoCancha { get; set; }
        public bool FixtureGenerado { get; set; }
        public decimal PrecioInscripcion { get; set; } = 50000;

        // 🔗 Equipos
        public ICollection<Equipo> Equipos { get; set; }

        // 🔗 Partidos (fixture real)
        public ICollection<Partido> Partidos { get; set; }
    }
}

