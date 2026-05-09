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
    public interface IAdminCommand
    {
        Task<AdministradorDto> CreateAsync(Administrador dto);
        Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto);
        Task<AdministradorResponse?> UpdateSimpleAsync(int id, UpdateAdministradorSimpleRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
