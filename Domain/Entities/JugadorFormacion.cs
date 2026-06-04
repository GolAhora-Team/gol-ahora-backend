namespace Domain.Entities
{
    public class JugadorFormacion
    {
        public int FormacionId { get; set; }
        public EquipoFormacion Formacion { get; set; }

        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }

        public bool EsTitular { get; set; }
        public int Posicion { get; set; }
        public bool EsCapitan { get; set; }
    }
}
