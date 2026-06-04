using System.Collections.Generic;

namespace Domain.Entities
{
    public class EquipoFormacion
    {
        public int Id { get; set; }
        
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }

        public string TipoCancha { get; set; }
        public string FormacionDefecto { get; set; }

        public ICollection<JugadorFormacion> JugadoresPosiciones { get; set; }
    }
}
