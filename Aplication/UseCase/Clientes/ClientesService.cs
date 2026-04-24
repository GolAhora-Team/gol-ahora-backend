using Aplication.DTOs.Clientes;
using Aplication.Interfaces.ICliente;
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

        public ClientesService(IClientesCommand command, IClientesQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<ClienteResponse> CreateCliente(CreateClienteRequest request)
        {
            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Dni = request.Dni,
                Genero = request.Genero,
                FechaNacimiento = request.FechaNacimiento,
                Telefono = request.Telefono,                
                Pais = request.Pais,
                Provincia = request.Provincia,
                Localidad = request.Localidad,
                CodigoPostal = request.CodigoPostal,
                Direccion = request.Direccion,
                ContactoEmergencia = request.ContactoEmergencia,
                Email = request.Email,
                ObraSocial = request.ObraSocial,
                AptoFisico = request.AptoFisico,
            };

            await _command.InsertCliente(cliente);
            return new ClienteResponse
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Genero = cliente.Genero,
                FechaNacimiento = cliente.FechaNacimiento,
                Telefono = cliente.Telefono,
                Pais= cliente.Pais,
                Provincia= cliente.Provincia,
                Localidad= cliente.Localidad,
                CodigoPostal= cliente.CodigoPostal,
                Direccion = cliente.Direccion,
                ContactoEmergencia = cliente.ContactoEmergencia,
                Email = cliente.Email,
                ObraSocial= cliente.ObraSocial,
                AptoFisico= cliente.AptoFisico
            };            
        }      
                

        public async Task<List<ClienteResponse>> GetAll()
        {            
            var clientes = await _query.GetListClientes();
                        
            var responseList = new List<ClienteResponse>();
            foreach (var cliente in clientes)
            {
                responseList.Add(new ClienteResponse
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Dni = cliente.Dni,
                    Genero = cliente.Genero,
                    FechaNacimiento = cliente.FechaNacimiento,
                    Telefono = cliente.Telefono,
                    Pais = cliente.Pais,
                    Provincia = cliente.Provincia,
                    Localidad = cliente.Localidad,
                    CodigoPostal = cliente.CodigoPostal,
                    Direccion = cliente.Direccion,
                    ContactoEmergencia = cliente.ContactoEmergencia,
                    Email = cliente.Email,
                    ObraSocial = cliente.ObraSocial,
                    AptoFisico = cliente.AptoFisico,
                    FechaAlta = cliente.FechaAlta,
                    FechaBaja = cliente.FechaBaja,
                    EsSocioActivo = cliente.EsSocioActivo
                });
            }

            return responseList;
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
            clienteOriginal.EsSocioActivo = request.EsSocioActivo;

            await _command.UpdateCliente(clienteOriginal);

            return new ClienteResponse
            {
                Id = clienteOriginal.Id,
                Nombre = clienteOriginal.Nombre,
                Apellido = clienteOriginal.Apellido,
                Dni = clienteOriginal.Dni,
                Genero = clienteOriginal.Genero,
                FechaNacimiento = clienteOriginal.FechaNacimiento,
                Telefono = clienteOriginal.Telefono,
                Pais = clienteOriginal.Pais,
                Provincia = clienteOriginal.Provincia,
                Localidad = clienteOriginal.Localidad,
                CodigoPostal = clienteOriginal.CodigoPostal,
                Direccion = clienteOriginal.Direccion,
                ContactoEmergencia = clienteOriginal.ContactoEmergencia,
                Email = clienteOriginal.Email,
                ObraSocial = clienteOriginal.ObraSocial,
                AptoFisico = clienteOriginal.AptoFisico,
                EsSocioActivo = clienteOriginal.EsSocioActivo
            };
        }

        public async Task<ClienteResponse> GetClienteById(int clienteId)
        {
            var cliente = await _query.GetClienteById(clienteId);

            if (cliente == null)
                throw new Exception("El cliente no existe");

            return new ClienteResponse
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Genero = cliente.Genero,
                FechaNacimiento = cliente.FechaNacimiento,
                Telefono = cliente.Telefono,
                Pais = cliente.Pais,
                Provincia = cliente.Provincia,
                Localidad = cliente.Localidad,
                CodigoPostal = cliente.CodigoPostal,
                Direccion = cliente.Direccion,
                ContactoEmergencia = cliente.ContactoEmergencia,
                Email = cliente.Email,
                ObraSocial = cliente.ObraSocial,
                AptoFisico = cliente.AptoFisico,
                FechaAlta = cliente.FechaAlta,
                FechaBaja = cliente.FechaBaja,
                EsSocioActivo = cliente.EsSocioActivo
            };
        }
                
        public async Task DeleteCliente(int id)
        {
            await _command.RemoveCliente(id);
        }
    }
}
