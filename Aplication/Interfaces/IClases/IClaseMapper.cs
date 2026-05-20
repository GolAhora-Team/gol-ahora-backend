using Aplication.DTOs.Request.Clase;
using Aplication.DTOs.Response.Clase;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IClases
{
    public interface IClaseMapper
    {
            Clase createClasetoRequest(ClaseCreateRequest request);
            ClaseCreateResponse ClaseCreateResponse(Clase clase);
            ClaseResponse ClaseToClaseResponse(Clase clase);
    }
}
