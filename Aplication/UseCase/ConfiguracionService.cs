using Aplication.DTOs;
using Aplication.Interfaces.IConfiguracion;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionQuery _query;
        private readonly IConfiguracionCommand _command;

        public ConfiguracionService(IConfiguracionQuery query, IConfiguracionCommand command)
        {
            _query = query;
            _command = command;
        }

        public async Task<ConfiguracionCancelacionesDto> GetConfiguracion()
        {
            var conf = await _query.GetConfiguracion();
            return new ConfiguracionCancelacionesDto
            {
                HorasAntelacionMinima = conf.HorasAntelacionMinima,
                PorcentajePenalizacion = conf.PorcentajePenalizacion
            };
        }

        public async Task<ConfiguracionCancelacionesDto> UpdateConfiguracion(ConfiguracionCancelacionesDto dto)
        {
            if (dto.HorasAntelacionMinima < 0)
                throw new ExceptionBadRequest("Las horas de antelación no pueden ser negativas.");
            if (dto.PorcentajePenalizacion < 0 || dto.PorcentajePenalizacion > 100)
                throw new ExceptionBadRequest("El porcentaje de penalización debe estar entre 0 y 100.");

            var conf = await _query.GetConfiguracion();
            conf.HorasAntelacionMinima = dto.HorasAntelacionMinima;
            conf.PorcentajePenalizacion = dto.PorcentajePenalizacion;

            await _command.UpdateConfiguracion(conf);

            return dto;
        }
    }
}
