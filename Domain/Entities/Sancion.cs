using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sancion
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public bool Suspendido { get; set; }

        public TipoTarjeta Tarjeta { get; set; }

        // 🔗 Jugador
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }
    }
}
