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

        public bool EsCapitan { get; set; }

        public bool EsTitular { get; set; }

        public int Posicion { get; set; }

        public EstadoJugador Estado { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔗 Equipo
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }

        // 🔗 Sanciones
        public ICollection<Sancion> Sanciones { get; set; }
    }
}
