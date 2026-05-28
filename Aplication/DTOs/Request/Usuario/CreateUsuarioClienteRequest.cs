using Aplication.DTOs.Request.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Usuario
{
    public class CreateUsuarioClienteRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CreateClienteRequest Cliente { get; set; }
    }
}
