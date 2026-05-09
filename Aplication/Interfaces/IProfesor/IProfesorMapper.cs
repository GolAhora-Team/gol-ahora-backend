using Aplication.DTOs;
using Aplication.DTOs.Request.Profesor;
using Aplication.DTOs.Response.Profesor;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IProfesor
{
    public interface IProfesorMapper
    {
        ProfesorResponse CreateProfesorResponse(Profesor profesor);
        ProfesorResponse CreateProfesorResponseFromDto(ProfesorDto dto);
        Profesor CreateProfesorToProfesorRequest(CreateProfesorRequest request);
    }
}
