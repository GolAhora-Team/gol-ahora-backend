using Aplication.DTOs;
using Aplication.DTOs.Request.Admin;
using Aplication.DTOs.Response.Admin;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IAdmin
{
    public interface IAdminMapper
    {
        AdministradorResponse CreateAdministradorResponse(Administrador admin);
        AdministradorResponse CreateAdministradorResponseFromDto(AdministradorDto dto);
        Administrador CreateAdminToRequest(CreateAdminRequest request);
    }
}
