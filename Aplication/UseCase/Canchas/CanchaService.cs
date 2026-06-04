using Aplication.DTOs.Request.Cancha;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICancha;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.UseCase.Canchas
{
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaCommand _command;
        private readonly ICanchaQuery _query;
        private readonly ICanchaMapper _mapper;

        public CanchaService(ICanchaCommand command, ICanchaQuery query, ICanchaMapper mapper)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
        }

        public async Task<CanchaResponse> CreateCancha(CreateCanchaRequest request)
        {
            var cancha = _mapper.CreateRequestToCancha(request);
            await _command.InsertCancha(cancha);
            return _mapper.CanchaToResponse(cancha);
        }

        public async Task DeleteCancha(int idCancha)
        {
            var exists = await _query.CanchaExists(idCancha);
            if (!exists)
            {
                throw new Exception("La cancha no existe.");
            }
            await _command.RemoveCancha(idCancha);
        }

        public async Task<List<CanchaResponse>> GetAll()
        {
            var canchas = await _query.GetListCancha();
            return _mapper.ListCanchaToResponse(canchas);
        }

        public async Task<CanchaResponse> GetCanchaById(int idCancha)
        {
            var cancha = await _query.GetCanchaById(idCancha);
            if (cancha == null)
            {
                throw new Exception("La cancha no fue encontrada.");
            }
            return _mapper.CanchaToResponse(cancha);
        }

        public async Task<List<CanchaResponse>> GetCanchasActivas()
        {
            var canchas = await _query.GetCanchasActivas();
            return _mapper.ListCanchaToResponse(canchas);
        }

        public async Task<List<CanchaResponse>> GetCanchasDisponibles(DateTime fecha, TimeSpan hora)
        {
            var canchas = await _query.GetCanchasDisponibles(fecha, hora);
            return _mapper.ListCanchaToResponse(canchas);
        }

        public async Task<CanchaResponse> UpdateCancha(int idCancha, UpdateCanchaRequest request)
        {
            var cancha = await _query.GetCanchaById(idCancha);
            if (cancha == null)
            {
                throw new Exception("La cancha a actualizar no existe.");
            }

            var canchaUpdated = _mapper.UpdateRequestToCancha(request, cancha);
            await _command.UpdateCancha(canchaUpdated);
            
            return _mapper.CanchaToResponse(canchaUpdated);
        }

        public async Task UpdatePreciosGlobal(UpdatePreciosRequest request)
        {
            var canchas = await _query.GetListCancha();
            foreach (var cancha in canchas)
            {
                bool updated = false;
                if (cancha.Tipo == TipoCancha.Futbol5 && request.PrecioF5 > 0)
                {
                    cancha.PrecioPorHora = request.PrecioF5;
                    updated = true;
                }
                else if (cancha.Tipo == TipoCancha.Futbol7 && request.PrecioF7 > 0)
                {
                    cancha.PrecioPorHora = request.PrecioF7;
                    updated = true;
                }
                else if (cancha.Tipo == TipoCancha.Futbol11 && request.PrecioF11 > 0)
                {
                    cancha.PrecioPorHora = request.PrecioF11;
                    updated = true;
                }

                if (updated)
                {
                    await _command.UpdateCancha(cancha);
                }
            }
        }
    }
}
