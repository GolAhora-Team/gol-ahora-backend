using System.Collections.Generic;

namespace Aplication.DTOs.Response
{
    public class EquipoFormacionResponse
    {
        public int Id { get; set; }
        public string TipoCancha { get; set; }
        public string FormacionDefecto { get; set; }
        public List<JugadorFormacionResponse> Jugadores { get; set; }
    }

    public class JugadorFormacionResponse
    {
        public int JugadorId { get; set; }
        public bool EsTitular { get; set; }
        public int Posicion { get; set; }
        public bool EsCapitan { get; set; }
    }
}
