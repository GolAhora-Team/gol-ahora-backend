using Aplication.Interfaces.IConfiguracion;
using Domain.Entities;
using Infraestructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ConfiguracionCommand : IConfiguracionCommand
    {
        private readonly AppDbContext _context;

        public ConfiguracionCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateConfiguracion(ConfiguracionCancelaciones configuracion)
        {
            _context.ConfiguracionCancelaciones.Update(configuracion);
            await _context.SaveChangesAsync();
        }
    }
}
