using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Administrador : Persona
    {
        public int Identificador { get; set; }

        public DateTime FechaAlta { get; set; }

        public bool PuedeFacturar { get; set; }
    }
}
