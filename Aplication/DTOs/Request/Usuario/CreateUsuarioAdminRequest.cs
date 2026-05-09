using Aplication.DTOs.Request.Admin;

namespace Aplication.DTOs.Request.Usuario
{
    public class CreateUsuarioAdminRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CreateAdminRequest Admin { get; set; }
    }
}
