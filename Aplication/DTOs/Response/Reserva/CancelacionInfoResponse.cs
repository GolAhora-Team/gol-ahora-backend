using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response.Reserva
{
    public class CancelacionInfoResponse
    {
        public int ReservaId { get; set; }
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string ClienteNombre { get; set; }
        public string CanchaNombre { get; set; }
        public string MetodoPago { get; set; }

        /// <summary>
        /// Monto original abonado por la reserva
        /// </summary>
        public decimal MontoOriginal { get; set; }

        /// <summary>
        /// Horas restantes hasta el turno
        /// </summary>
        public double HorasRestantes { get; set; }

        /// <summary>
        /// Horas mínimas de antelación configuradas
        /// </summary>
        public int HorasAntelacionMinima { get; set; }

        /// <summary>
        /// true si la cancelación está dentro del plazo (sin penalización)
        /// </summary>
        public bool DentroDePlazo { get; set; }

        /// <summary>
        /// Porcentaje de penalización aplicable (0 si dentro de plazo)
        /// </summary>
        public decimal PorcentajePenalizacion { get; set; }

        /// <summary>
        /// Monto que se descuenta por penalización
        /// </summary>
        public decimal MontoPenalizacion { get; set; }

        /// <summary>
        /// Monto a reintegrar al cliente
        /// </summary>
        public decimal MontoReintegro { get; set; }
    }
}
