using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CreateClienteRequest
    {
        public bool EsSocioActivo { get; set; }

        public string ObraSocial { get; set; }

        public bool AptoFisico { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaBaja { get; set; }

        public Jugador Jugador { get; set; }
    }
}
