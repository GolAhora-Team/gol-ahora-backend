using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IUsuario
{
    public interface IUsuarioMapper
    {
        Usuario CreateUsuario(CreateUsuarioClienteRequest request, int personaId);

        Usuario CreateUsuario(CreateUsuarioAdminRequest request, int personaId);

        UsuarioClienteResponse CreateUsuarioResponse(int id, string nombre, string apellido);

        UsuarioAdminResponse CreateUsuarioAdminResponse(int id, string nombre, string apellido, DateTime fechaAlta);
    }
}
