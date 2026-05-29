using System;

namespace Domain.Entities
{
    public class Reporte
    {
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; }
        public string FileName { get; set; }
        public string Html { get; set; }
    }
}
