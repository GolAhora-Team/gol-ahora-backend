using Aplication.DTOs.Request.Cancha;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICancha;
using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Aplication.Mappers
{
    public class CanchaMapper : ICanchaMapper
    {
        public Cancha CreateRequestToCancha(CreateCanchaRequest request)
        {
            return new Cancha
            {
                Nombre = request.Nombre,
                Disponibilidad = request.Disponibilidad,
                Tipo = (TipoCancha)request.Tipo,
                Superficie = request.Superficie, // Assuming it's an int mapping to some enum or just int
                Capacidad = request.Capacidad,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                DuracionMax = request.DuracionMax,
                Estado = (EstadoCancha)request.Estado,
                PrecioPorHora = request.PrecioPorHora
            };
        }

        public Cancha UpdateRequestToCancha(UpdateCanchaRequest request, Cancha cancha)
        {
            cancha.Nombre = request.Nombre;
            cancha.Disponibilidad = request.Disponibilidad;
            cancha.Tipo = (TipoCancha)request.Tipo;
            cancha.Superficie = request.Superficie;
            cancha.Capacidad = request.Capacidad;
            cancha.HoraInicio = request.HoraInicio;
            cancha.HoraFin = request.HoraFin;
            cancha.DuracionMax = request.DuracionMax;
            cancha.Estado = (EstadoCancha)request.Estado;
            cancha.PrecioPorHora = request.PrecioPorHora;
            return cancha;
        }

        public CanchaResponse CanchaToResponse(Cancha cancha)
        {
            return new CanchaResponse
            {
                Id = cancha.Id,
                Nombre = cancha.Nombre,
                Disponibilidad = cancha.Disponibilidad,
                Tipo = cancha.Tipo.ToString(),
                Superficie = cancha.Superficie.ToString(),
                Capacidad = cancha.Capacidad,
                HoraInicio = cancha.HoraInicio,
                HoraFin = cancha.HoraFin,
                DuracionMax = cancha.DuracionMax,
                Estado = cancha.Estado.ToString(),
                PrecioPorHora = cancha.PrecioPorHora
            };
        }

        public List<CanchaResponse> ListCanchaToResponse(List<Cancha> canchas)
        {
            return canchas.Select(c => CanchaToResponse(c)).ToList();
        }
    }
}
