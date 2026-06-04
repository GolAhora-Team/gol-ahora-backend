using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Partido
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan Hora { get; set; }

        public string Arbitro { get; set; }

        public int GolesLocal { get; set; }

        public int GolesVisitante { get; set; }

        public int? PenalesLocal { get; set; }

        public int? PenalesVisitante { get; set; }

        public EstadoPartido Estado { get; set; }

        public int Jornada { get; set; }

        public FaseTorneo Fase { get; set; }

        // 🔗 Equipos
        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; }

        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; }

        // 🔗 Competición
        public int CompeticionId { get; set; }
        public Competicion Competicion { get; set; }

        // 🔗 Ganador (opcional)
        public int? GanadorId { get; set; }
        public Equipo Ganador { get; set; }

        // 🔗 Cambios
        public ICollection<Cambio> Cambios { get; set; }
    }
}
