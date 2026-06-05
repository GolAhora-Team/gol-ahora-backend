using Aplication.Interfaces.IConfiguracion;
using Domain.Entities;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ConfiguracionQuery : IConfiguracionQuery
    {
        private readonly AppDbContext _context;

        public ConfiguracionQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracionCancelaciones> GetConfiguracion()
        {
            // Siempre asumimos que el Id es 1 porque hay una sola fila de configuración
            var conf = await _context.ConfiguracionCancelaciones.FirstOrDefaultAsync(c => c.Id == 1);
            if (conf == null)
            {
                conf = new ConfiguracionCancelaciones { Id = 1, HorasAntelacionMinima = 24, PorcentajePenalizacion = 50 };
                _context.ConfiguracionCancelaciones.Add(conf);
                await _context.SaveChangesAsync();
            }
            return conf;
        }
    }
}
