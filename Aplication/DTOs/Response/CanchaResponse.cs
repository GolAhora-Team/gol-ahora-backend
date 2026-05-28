using System;

namespace Aplication.DTOs.Response
{
    public class CanchaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Disponibilidad { get; set; }
        public string Tipo { get; set; }
        public string Superficie { get; set; }
        public int Capacidad { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int DuracionMax { get; set; }
        public string Estado { get; set; }
        public decimal PrecioPorHora { get; set; }
    }
}
