using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IUsuario;
using Domain.Exceptions;

namespace Aplication.UseCase
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioCommand _usuariocommand;
        private readonly IUsuarioQuery _usuarioquery;
        private readonly IUsuarioMapper _usuariomapper;
        private readonly IClienteMapper _clientemapper;
        private readonly IClientesCommand _clienteCommand;

        public UsuarioService(IUsuarioCommand usuariocommand, IUsuarioQuery usuarioquery, IClienteMapper clientemapper, IClientesCommand clienteCommand)
        {
            _usuariocommand = usuariocommand;
            _usuarioquery = usuarioquery;
            _clientemapper = clientemapper;
            _clienteCommand = clienteCommand;
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
    }
}
