using System;

namespace Aplication.DTOs.Request.Cancha
{
    public class UpdateCanchaRequest
    {
        public string Nombre { get; set; }
        public bool Disponibilidad { get; set; }
        public int Tipo { get; set; }
        public int Superficie { get; set; }
        public int Capacidad { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int DuracionMax { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPorHora { get; set; }
    }
}
