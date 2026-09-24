using Aplication.DTOs;
using Aplication.DTOs.Request.Admin;
using Aplication.DTOs.Response.Admin;
using Aplication.Interfaces.IAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class AdministradorService : IAdminService
    {
        private readonly IAdminQuery _query;
        private readonly IAdminCommand _command;

        public AdministradorService(IAdminQuery query, IAdminCommand command)
        {
            _query = query;
            _command = command;
        }

        public async Task<IEnumerable<AdministradorDto>> GetAllAsync()
            => await _query.GetAllAsync();

        public async Task<AdministradorDto?> GetByIdAsync(int id)
            => await _query.GetByIdAsync(id);

        public async Task<AdministradorDto?> GetByDniAsync(int dni)
            => await _query.GetByDniAsync(dni);

        public async Task<AdministradorDto?> GetByIdentificadorAsync(int identificador)
            => await _query.GetByIdentificadorAsync(identificador);

        public async Task<IEnumerable<AdministradorDto>> GetFacturadoresAsync()
            => await _query.GetFacturadoresAsync();

        public async Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto)
            => await _command.UpdateAsync(id, dto);

        public async Task<AdministradorResponse?> UpdateSimpleAsync(int id, UpdateAdministradorSimpleRequest request)
            => await _command.UpdateSimpleAsync(id, request);

        public async Task<bool> DeleteAsync(int id)
            => await _command.DeleteAsync(id);
    }
}
