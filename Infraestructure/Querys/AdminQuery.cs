using Aplication.DTOs;
using Aplication.Interfaces.IAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class AdminQuery : IAdminQuery
    {
        public Task<IEnumerable<AdministradorDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AdministradorDto?> GetByDniAsync(int dni)
        {
            throw new NotImplementedException();
        }

        public Task<AdministradorDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdministradorDto?> GetByIdentificadorAsync(int identificador)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdministradorDto>> GetFacturadoresAsync()
        {
            throw new NotImplementedException();
        }
    }
}
