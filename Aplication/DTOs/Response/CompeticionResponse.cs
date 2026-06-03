using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response
{
    public class CompeticionResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public TipoCompeticion Tipo { get; set; }

        public string Descripcion { get; set; }

        public int CantidadEquipos { get; set; }

        public EstadoCompeticion Estado { get; set; }

        public int CantInscriptos { get; set; }
    }
}
