using Aplication.DTOs.Request.Profesor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Usuario
{
    public class CreateUsuarioProfesorRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public CreateProfesorRequest request { get; set; }
    }
}
