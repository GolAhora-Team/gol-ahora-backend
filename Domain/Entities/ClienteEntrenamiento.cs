using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClienteEntrenamiento
    {
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int EntrenamientoId { get; set; }
        public Entrenamiento Entrenamiento { get; set; }
    }
}
