using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;
using Aplication.DTOs.Request.Admin;
using Aplication.DTOs.Response.Admin;
using Aplication.Interfaces.IAdmin;
using Domain.Entities;
using Aplication.Interfaces.IAdmin;

namespace Infraestructure.Command
{
    public class AdministradorCommand : IAdminCommand
    {
        private readonly AppDbContext _context;
        private readonly IAdminMapper _mapper;

        public AdministradorCommand(AppDbContext context, IAdminMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<AdministradorDto> CreateAsync(AdministradorDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var admin = await _context.Administradores.FindAsync(id);
            if (admin == null) return false;

            _context.Administradores.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<AdministradorResponse?> UpdateSimpleAsync(int id, UpdateAdministradorSimpleRequest request)
        {
            var admin = await _context.Administradores.FindAsync(id);
            if (admin == null) return null;

            admin.Telefono = request.Telefono;
            admin.Direccion = request.Direccion;
            admin.Localidad = request.Localidad;
            admin.CodigoPostal = request.CodigoPostal;
            admin.Provincia = request.Provincia;
            admin.Pais = request.Pais;
            admin.ContactoEmergencia = request.ContactoEmergencia;
            admin.Email = request.Email;

            await _context.SaveChangesAsync();

            return _mapper.CreateAdministradorResponse(admin);
        }
    }
}
