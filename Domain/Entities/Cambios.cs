using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cambio
    {
        public int Id { get; set; }

        public TimeSpan Hora { get; set; }

        // 🔗 Jugador que sale
        public int JugadorSaleId { get; set; }
        public Jugador JugadorSale { get; set; }

        // 🔗 Jugador que entra
        public int JugadorEntraId { get; set; }
        public Jugador JugadorEntra { get; set; }

        // 🔗 Partido
        public int PartidoId { get; set; }
        public Partido Partido { get; set; }
    }
}
