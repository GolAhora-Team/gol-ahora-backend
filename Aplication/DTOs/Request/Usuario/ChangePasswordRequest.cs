namespace Aplication.DTOs.Request.Usuario
{
    public class ChangePasswordRequest
    {
        public int IdUsuario { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
