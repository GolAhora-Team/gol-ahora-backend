using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Jugador
    {
        public int Id { get; set; }

        public int Numero { get; set; }



        public EstadoJugador Estado { get; set; }

        // 🔗 Cliente (obligatorio)
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        
        public ICollection<Sancion> Sanciones { get; set; }

        public ICollection<JugadorFormacion> FormacionesPosiciones { get; set; }

        // 🔗 Equipo (obligatorio)        
        public int EquipoId { get; set; }
        public string EquipoNombre { get; set; }
        public Equipo Equipo { get; set; }
    }
}
