using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cliente: Persona
    {
        public bool EsSocioActivo { get; set; }

        public string ObraSocial { get; set; }

        public bool AptoFisico { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaBaja { get; set; }

        public byte[]? AptoMedicoArchivo { get; set; }

        public DateTime? AptoMedicoFechaInicio { get; set; }

        public DateTime? AptoMedicoFechaFin { get; set; }

        public Jugador Jugador { get; set; }

        // Relaciones
        public ICollection<Reserva> Reservas { get; set; }
    }
}
