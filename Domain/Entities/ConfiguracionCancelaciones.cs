using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ConfiguracionCancelaciones
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Horas de antelación mínima para cancelar sin penalización (ej. 24)
        /// </summary>
        public int HorasAntelacionMinima { get; set; }
        
        /// <summary>
        /// Porcentaje de penalización si se cancela fuera del plazo (ej. 50 para 50%)
        /// </summary>
        public decimal PorcentajePenalizacion { get; set; }
    }
}
