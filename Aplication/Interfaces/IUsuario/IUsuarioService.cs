using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
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
        Task<UsuarioAdminResponse> CreateUsuarioAdmin(CreateUsuarioAdminRequest usuario);
        Task<UsuarioProfesorResponse> CreateUsuarioProfesor(CreateUsuarioProfesorFormRequest usuario);
        Task<UsuarioLogginResponse> LogginUsuario(LoginRequest loginRequest);
        Task<bool> ChangePassword(ChangePasswordRequest request);
        Task<List<string>> CheckAvailability(int dni, string email, string username);
        Task<bool> ForgotPassword(string email);
        Task ResetPassword(ResetPasswordRequest request);
        Task<bool> ValidateResetToken(string token);
        Task UploadAptoMedico(UploadAptoMedicoRequest request);
    }
}
