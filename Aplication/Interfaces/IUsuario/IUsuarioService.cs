using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IUsuario
{
    public interface IUsuarioService
    {
        Task<UsuarioClienteResponse> CreateUsuarioCliente(CreateUsuarioClienteRequest usuario);
    }
}
