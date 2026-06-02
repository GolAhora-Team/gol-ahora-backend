using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
using Aplication.Interfaces.IAdmin;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IProfesor;
using Aplication.Interfaces.IUsuario;
using Domain.Exceptions;
using System.Net;
using System.Net.Mail;

namespace Aplication.UseCase
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioCommand _usuariocommand;
        private readonly IUsuarioQuery _usuarioQuery;
        private readonly IUsuarioMapper _usuariomapper;
        private readonly IClienteMapper _clientemapper;
        private readonly IClientesCommand _clienteCommand;
        private readonly IClientesQuery _clientesQuery;
        private readonly IAdminCommand _adminCommand;
        private readonly IAdminMapper _adminMapper;
        private readonly IAdminQuery _adminQuery;
        private readonly IProfesorCommand _profesorCommand;
        private readonly IProfesorMapper _profesorMapper;
        private readonly IProfesorQuery _profesorQuery;

        public UsuarioService(IUsuarioCommand usuariocommand, IUsuarioMapper usuariomapper, IClienteMapper clientemapper, IClientesCommand clienteCommand, IAdminMapper adminMapper, IAdminCommand adminCommand, IProfesorMapper profesorMapper, IProfesorCommand profesorCommand, IUsuarioQuery usuarioQuery, IProfesorQuery profesorQuery, IClientesQuery clientesQuery, IAdminQuery adminQuery)
        {
            _usuariocommand = usuariocommand;
            _usuariomapper = usuariomapper;
            _clientemapper = clientemapper;
            _clienteCommand = clienteCommand;
            _adminMapper = adminMapper;
            _adminCommand = adminCommand;
            _profesorMapper = profesorMapper;
            _profesorCommand = profesorCommand;
            _usuarioQuery = usuarioQuery;
            _profesorQuery = profesorQuery;
            _clientesQuery = clientesQuery;
            _adminQuery = adminQuery;
        }

        public async Task<UsuarioClienteResponse> CreateUsuarioCliente(CreateUsuarioClienteRequest usuario)
        {
            if (usuario.Cliente.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.Cliente.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            var clienteEntity = _clientemapper.CreateCliente(usuario.Cliente);
            await _clienteCommand.InsertCliente(clienteEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, clienteEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioResponse(usuarioEntity.Id, clienteEntity.Nombre, clienteEntity.Apellido);
        }

        public async Task<UsuarioAdminResponse> CreateUsuarioAdmin(CreateUsuarioAdminRequest usuario)
        {
            if (usuario.Admin.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.Admin.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            var adminEntity = _adminMapper.CreateAdminToRequest(usuario.Admin);
            await _adminCommand.CreateAsync(adminEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, adminEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioAdminResponse(usuarioEntity.Id, adminEntity.Nombre, adminEntity.Apellido, adminEntity.FechaAlta);
        }

        public async Task<UsuarioProfesorResponse> CreateUsuarioProfesor(CreateUsuarioProfesorRequest usuario)
        {
            if (usuario.request.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.request.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            if (!string.IsNullOrEmpty(usuario.request.CertificadoBase64))
            {
                var base64Data = usuario.request.CertificadoBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }
                // Validar tamaño: máximo 4MB
                // Fórmula aproximada de base64 a bytes: (longitud base64 * 3) / 4
                var sizeInBytes = (base64Data.Length * 3) / 4;
                if (sizeInBytes > 4 * 1024 * 1024)
                {
                    throw new ExceptionBadRequest("El certificado excede el límite máximo de 4 MB.");
                }
            }

            var profesorEntity = _profesorMapper.CreateProfesorToProfesorRequest(usuario.request);
            await _profesorCommand.CreateProfesor(profesorEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, profesorEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioProfesorResponse(usuarioEntity.Id, profesorEntity.Nombre, profesorEntity.Apellido, profesorEntity.Especialidad);
        }

        public async Task<UsuarioLogginResponse> LogginUsuario(LoginRequest loginRequest)
        {
            var usuarioEntity = await _usuarioQuery.GetByIdentifierAndPassword(loginRequest.Email, loginRequest.Password);
            if (usuarioEntity == null)
            {
                throw new ExceptionNotFound("Usuario no encontrado.");
            }

            if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Cliente)
            {
                var clienteEntity = await _clientesQuery.GetClienteById(usuarioEntity.PersonaId);
                if (clienteEntity == null)
                {
                    throw new ExceptionNotFound("Cliente no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = clienteEntity.Id,
                    Nombre = clienteEntity.Nombre,
                    Apellido = clienteEntity.Apellido
                };
            }
            else if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Administrador)
            {
                var adminEntity = await _adminQuery.GetByIdAsync(usuarioEntity.PersonaId);
                if (adminEntity == null)
                {
                    throw new ExceptionNotFound("Admin no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = adminEntity.Id,
                    Nombre = adminEntity.Nombre,
                    Apellido = adminEntity.Apellido,
                    Identificador = adminEntity.Identificador
                };
            }
            else if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Profesor)
            {
                var profesorEntity = await _profesorQuery.GetByIdAsync(usuarioEntity.PersonaId);
                if (profesorEntity == null)
                {
                    throw new ExceptionNotFound("Profesor no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = profesorEntity.Id,
                    Nombre = profesorEntity.Nombre,
                    Apellido = profesorEntity.Apellido
                };
            }
            else
            {
                throw new ExceptionBadRequest("Tipo de usuario no válido.");
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var usuario = await _usuarioQuery.GetById(request.IdUsuario);
            if (usuario == null)
            {
                throw new ExceptionNotFound("Usuario no encontrado.");
            }

            if (usuario.PasswordHash != request.CurrentPassword)
            {
                throw new ExceptionBadRequest("La contraseña actual es incorrecta.");
            }

            usuario.PasswordHash = request.NewPassword;
            await _usuariocommand.Update(usuario);

            return true;
        }

        public async Task<List<string>> CheckAvailability(int dni, string email, string username)
        {
            return await _usuarioQuery.CheckUniqueness(dni, email, username);
        }


        public async Task<bool> ForgotPassword(string email)
        {
            var usuarioEntity = await _usuarioQuery.GetByEmail(email);
            if (usuarioEntity == null)
            {
                // Por seguridad es mejor no revelar si el email existe o no, pero seguiremos el patrón de la app.
                throw new ExceptionNotFound("Usuario no encontrado con ese correo.");
            }

            // Generar token y expiración
            var token = Guid.NewGuid().ToString();
            usuarioEntity.ResetToken = token;
            usuarioEntity.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _usuariocommand.Update(usuarioEntity);

            // Enviar email
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("complejogolahora@gmail.com", "tgcf xugs czdh ekpx"),
                    EnableSsl = true,
                };

                var frontendUrl = $"http://localhost:8081/NuevaClave?token={token}";
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("complejogolahora@gmail.com", "Complejo Gol Ahora"),
                    Subject = "Recuperación de Contraseña",
                    Body = $"Hola,\n\nHas solicitado restablecer tu contraseña. Por favor, haz clic en el siguiente enlace:\n\n{frontendUrl}\n\nSi no solicitaste este cambio, puedes ignorar este correo.\n\nEste enlace expira en 1 hora.",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Ocurrió un error al enviar el correo de recuperación.");
            }
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var usuario = await _usuarioQuery.GetByResetToken(request.Token);
            if (usuario == null)
            {
                throw new ExceptionNotFound("Token inválido o expirado.");
            }

            if (usuario.ResetTokenExpires < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("El token de recuperación ha expirado.");
            }

            if (usuario.PasswordHash == request.NewPassword)
            {
                throw new ExceptionBadRequest("La nueva contraseña no puede ser igual a la anterior.");
            }

            usuario.PasswordHash = request.NewPassword;
            usuario.ResetToken = null;
            usuario.ResetTokenExpires = null;

            await _usuariocommand.Update(usuario);
        }

        public async Task UploadAptoMedico(UploadAptoMedicoRequest request)
        {
            if (string.IsNullOrEmpty(request.ArchivoBase64))
            {
                throw new ExceptionBadRequest("No se ha proporcionado ningún archivo.");
            }

            var base64Data = request.ArchivoBase64;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            var sizeInBytes = (base64Data.Length * 3) / 4;

            if (sizeInBytes > 2 * 1024 * 1024)
            {
                throw new ExceptionBadRequest("El tamaño del archivo no puede exceder los 2MB.");
            }

            if (request.FechaFin <= request.FechaInicio)
            {
                throw new ExceptionBadRequest("La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            var cliente = await _clientesQuery.GetClienteById(request.ClienteId);
            if (cliente == null)
            {
                throw new ExceptionNotFound("Cliente no encontrado.");
            }

            cliente.AptoMedicoArchivo = Convert.FromBase64String(base64Data);
            cliente.AptoMedicoFechaInicio = request.FechaInicio;
            cliente.AptoMedicoFechaFin = request.FechaFin;
            cliente.AptoFisico = true;

            await _clienteCommand.UpdateCliente(cliente);
        }
    }
}
