using Aplication.DTOs;
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
        Task<AdministradorDto> CreateAsync(AdministradorDto dto);
        Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
