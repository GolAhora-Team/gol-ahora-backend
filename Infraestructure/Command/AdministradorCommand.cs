using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;
using Aplication.Interfaces.IAdmin;

namespace Infraestructure.Command
{
    public class AdministradorCommand : IAdminCommand
    {
        public Task<AdministradorDto> CreateAsync(AdministradorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
