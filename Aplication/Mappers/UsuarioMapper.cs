using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
using Aplication.Interfaces.IUsuario;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class UsuarioMapper : IUsuarioMapper
    {
        public Usuario CreateUsuario(CreateUsuarioClienteRequest request, int personaId)
        {
            return new Usuario
            {
                Email = request.Email,
                PasswordHash = request.Password,
                TipoUsuario = Domain.Enums.TipoUsuario.Cliente,
                PersonaId = personaId
            };
        }

        public Usuario CreateUsuario(CreateUsuarioAdminRequest request, int personaId)
        {
            return new Usuario
            {
                Email = request.Email,
                PasswordHash = request.Password,
                TipoUsuario = Domain.Enums.TipoUsuario.Administrador,
                PersonaId = personaId
            };
        }

        public UsuarioClienteResponse CreateUsuarioResponse(int id, string nombre, string apellido)
        {
            return new UsuarioClienteResponse
            {
                IdUsuario = id,
                Nombre = nombre,
                Apellido = apellido
            };
        }

        public UsuarioAdminResponse CreateUsuarioAdminResponse(int id, string nombre, string apellido,DateTime fechaAlta)
        {
            return new UsuarioAdminResponse
            {
                IdUsuario = id,
                Nombre = nombre,
                Apellido = apellido,
                FechaAlta = fechaAlta
            };
        }
    }
}
