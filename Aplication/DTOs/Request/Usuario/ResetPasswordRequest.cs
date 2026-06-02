using System;

namespace Aplication.DTOs.Request.Usuario
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
