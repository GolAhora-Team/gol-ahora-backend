using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IConfiguracion
{
    public interface IConfiguracionCommand
    {
        Task UpdateConfiguracion(ConfiguracionCancelaciones configuracion);
    }
}
