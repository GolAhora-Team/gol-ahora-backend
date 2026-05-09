using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
using Aplication.Interfaces.IAdmin;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IProfesor;
using Aplication.Interfaces.IUsuario;
using Domain.Exceptions;

namespace Aplication.UseCase
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioCommand _usuariocommand;
        private readonly IUsuarioMapper _usuariomapper;
        private readonly IClienteMapper _clientemapper;
        private readonly IClientesCommand _clienteCommand;
        private readonly IAdminCommand _adminCommand;
        private readonly IAdminMapper _adminMapper;
        private readonly IProfesorCommand _profesorCommand;
        private readonly IProfesorMapper _profesorMapper;

        public UsuarioService(IUsuarioCommand usuariocommand, IClienteMapper clientemapper, IClientesCommand clienteCommand, IAdminMapper adminMapper, IAdminCommand adminCommand, IProfesorMapper profesorMapper, IProfesorCommand profesorCommand)
        {
            _usuariocommand = usuariocommand;
            _clientemapper = clientemapper;
            _clienteCommand = clienteCommand;
            _adminMapper = adminMapper;
            _adminCommand = adminCommand;
            _profesorMapper = profesorMapper;
            _profesorCommand = profesorCommand;
        }

        public async Task<UsuarioClienteResponse> CreateUsuarioCliente(CreateUsuarioClienteRequest usuario)
        {
            if (usuario.Cliente.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
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

            var profesorEntity = _profesorMapper.CreateProfesorToProfesorRequest(usuario.request);
            await _profesorCommand.CreateProfesor(profesorEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, profesorEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioProfesorResponse(usuarioEntity.Id, profesorEntity.Nombre, profesorEntity.Apellido, profesorEntity.Especialidad);
        }
    }
}
