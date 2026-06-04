using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response
{
    public class EquipoResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CantidadMaxJugadores { get; set; }

        public string Descripcion { get; set; }

        public string ColorPrimario { get; set; }
        public string ColorSecundario { get; set; }

        public int CantPuntos { get; set; }

        public int PartidosJugados { get; set; }

        public int GolesAFavor { get; set; }

        public int GolesEnContra { get; set; }

        public int? CompeticionId { get; set; }

        public int? CreadoPorClienteId { get; set; }
        public string? CreadoPorNombre { get; set; }
    }
}
