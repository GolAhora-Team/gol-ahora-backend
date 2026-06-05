using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Entrenamiento
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public DateTime Fecha { get; set; }

        public int CupoMaximo { get; set; }

        // 🔗 Profesor
        public int? ProfesorId { get; set; }
        public Profesor? Profesor { get; set; }

        // 🔗 Cancha (opcional)
        public int? CanchaId { get; set; }
        public Cancha Cancha { get; set; }

        // 🔥 relación muchos a muchos
        public ICollection<ClienteEntrenamiento> Clientes { get; set; }
    }
}
