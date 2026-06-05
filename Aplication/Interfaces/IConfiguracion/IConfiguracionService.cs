using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IConfiguracion
{
    public interface IConfiguracionService
    {
        Task<ConfiguracionCancelacionesDto> GetConfiguracion();
        Task<ConfiguracionCancelacionesDto> UpdateConfiguracion(ConfiguracionCancelacionesDto dto);
    }
}
