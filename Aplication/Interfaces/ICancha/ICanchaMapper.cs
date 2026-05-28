using Aplication.DTOs.Request.Cancha;
using Aplication.DTOs.Response;
using Domain.Entities;
using System.Collections.Generic;

namespace Aplication.Interfaces.ICancha
{
    public interface ICanchaMapper
    {
        Cancha CreateRequestToCancha(CreateCanchaRequest request);
        Cancha UpdateRequestToCancha(UpdateCanchaRequest request, Cancha cancha);
        CanchaResponse CanchaToResponse(Cancha cancha);
        List<CanchaResponse> ListCanchaToResponse(List<Cancha> canchas);
    }
}
