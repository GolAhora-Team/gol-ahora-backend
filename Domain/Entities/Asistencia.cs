using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Asistencia
    {
        public int Id { get; set; }

        public bool Presente { get; set; }

        public DateTime Fecha { get; set; }

        // 🔗 Clase
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }

        // 🔗 Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
