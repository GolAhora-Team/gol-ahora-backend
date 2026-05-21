using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response
{
    public class PartidoResponse
    {
        public int Id { get; set; } 

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Arbitro { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public EstadoPartido Estado { get; set; }
        public int Jornada { get; set; }
        public FaseTorneo Fase { get; set; }

        
        public int EquipoLocalId { get; set; }
        public string EquipoLocalNombre { get; set; } 

        
        public int EquipoVisitanteId { get; set; }
        public string EquipoVisitanteNombre { get; set; }

        
        public int CompeticionId { get; set; }
        public string CompeticionNombre { get; set; }

        
        public int? GanadorId { get; set; }
        public string GanadorNombre { get; set; }
        
    }
}
