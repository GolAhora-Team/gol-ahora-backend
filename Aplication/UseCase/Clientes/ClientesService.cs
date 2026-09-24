using Aplication.DTOs.Request.Cliente;
using Aplication.Interfaces;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.INotificacion;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Aplication.UseCase.Clientes
{
    public class ClientesService : IClienteServices
    {
        private readonly IClientesCommand _command;
        private readonly IClientesQuery _query;
        private readonly IClienteMapper _mapper;
        private readonly INotificacionService _notificacionService;

        public ClientesService(IClientesCommand command, IClientesQuery query, IClienteMapper mapper, INotificacionService notificacionService)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
            _notificacionService = notificacionService;
        }

        public async Task<ClienteResponse> CreateCliente(CreateClienteRequest request)
        {
            var cliente = _mapper.CreateCliente(request);

            await _command.InsertCliente(cliente);

            cliente = await _query.GetClienteById(cliente.Id);
            return _mapper.CreateClienteResponse(cliente);               
        }                      

        public async Task<List<ClienteResponse>> GetAll()
        {            
            var clientes = await _query.GetListClientes();

            return clientes.Select(cliente => _mapper.CreateClienteResponse(cliente)).ToList();
        }
        
        public async Task<ClienteResponse> UpdateCliente(int clienteId, UpdateClienteRequest request)
        {
            var clienteOriginal = await _query.GetClienteById(clienteId);

            if (clienteOriginal == null)
                throw new Exception("El cliente no existe");

            clienteOriginal.Nombre = request.Nombre;
            clienteOriginal.Apellido = request.Apellido;
            clienteOriginal.Dni = request.Dni;
            clienteOriginal.Genero = request.Genero;
            clienteOriginal.FechaNacimiento = request.FechaNacimiento;
            clienteOriginal.Telefono = request.Telefono;
            clienteOriginal.Pais = request.Pais;
            clienteOriginal.Provincia = request.Provincia;
            clienteOriginal.Localidad = request.Localidad;
            clienteOriginal.CodigoPostal = request.CodigoPostal;
            clienteOriginal.Direccion = request.Direccion;
            clienteOriginal.ContactoEmergencia = request.ContactoEmergencia;
            clienteOriginal.Email = request.Email;
            clienteOriginal.ObraSocial = request.ObraSocial;
            clienteOriginal.AptoFisico = request.AptoFisico;
            bool eraSocio = clienteOriginal.EsSocioActivo;

            clienteOriginal.EsSocioActivo = request.EsSocioActivo;

            if (!eraSocio && request.EsSocioActivo)
            {
                await _notificacionService.CrearNotificacionGeneral($"El alumno {clienteOriginal.Nombre} {clienteOriginal.Apellido} ha pagado para ser socio.", "ADMIN,PERSONAL", "General");
            }
            else if (eraSocio && !request.EsSocioActivo)
            {
                await _notificacionService.CrearNotificacionGeneral($"El alumno {clienteOriginal.Nombre} {clienteOriginal.Apellido} ha dejado de ser socio.", "ADMIN,PERSONAL", "General");
            }

            await _command.UpdateCliente(clienteOriginal);

            clienteOriginal = await _query.GetClienteById(clienteOriginal.Id);
            return _mapper.CreateClienteResponse(clienteOriginal);
        }

        public async Task<ClienteResponse> GetClienteById(int clienteId)
        {
            var cliente = await _query.GetClienteById(clienteId);

            if (cliente == null)
                throw new Exception("El cliente no existe");

            return _mapper.CreateClienteResponse(cliente);
        }

        public async Task<byte[]?> GetAptoMedico(int clienteId)
        {
            var cliente = await _query.GetClienteById(clienteId);
            return cliente?.AptoMedicoArchivo;
        }

        public async Task DeleteAptoMedico(int clienteId)
        {
            var cliente = await _query.GetClienteById(clienteId);
            if (cliente == null)
                throw new Exception("El cliente no existe");

            cliente.AptoMedicoArchivo = null;
            cliente.AptoMedicoFechaInicio = null;
            cliente.AptoMedicoFechaFin = null;
            cliente.AptoFisico = false;

            await _command.UpdateCliente(cliente);
        }
                
        public async Task<ClienteResponse> DeleteCliente(int id)
        {
            var cliente = await _query.GetClienteById(id);
            if (cliente == null)
            {
                throw new Exception("Cliente no encontrado");
            }

            await _command.RemoveCliente(cliente.Id);
            return _mapper.CreateClienteResponse(cliente);
        }
    }
}
