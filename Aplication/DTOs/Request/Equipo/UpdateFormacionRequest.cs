using System.Collections.Generic;

namespace Aplication.DTOs.Request.Equipo
{
    public class UpdateFormacionRequest
    {
        public string TipoCancha { get; set; }
        public string FormacionDefecto { get; set; }
        public int? CapitanId { get; set; }
        public List<JugadorPosicionRequest> Jugadores { get; set; }
    }

    public class JugadorPosicionRequest
    {
        public int JugadorId { get; set; }
        public int? Posicion { get; set; } // 0: None, 1: Arquero, 2: Defensor, 3: Mediocampista, 4: Delantero
        public bool EsTitular { get; set; }
    }
}
