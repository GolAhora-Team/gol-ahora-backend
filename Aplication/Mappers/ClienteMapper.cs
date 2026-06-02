using Aplication.DTOs.Request.Cliente;
using Aplication.Interfaces.ICliente;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class ClienteMapper : IClienteMapper
    {
        public Cliente CreateCliente(CreateClienteRequest cliente)
        {
            return new Cliente
            {                
                Dni = cliente.Dni,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Genero = cliente.Genero,
                FechaNacimiento = cliente.FechaNacimiento,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                Localidad = cliente.Localidad,
                CodigoPostal = cliente.CodigoPostal,
                Provincia = cliente.Provincia,
                Pais = cliente.Pais,
                ContactoEmergencia = cliente.ContactoEmergencia,
                Email = cliente.Email,
                ObraSocial = cliente.ObraSocial,
                AptoFisico = cliente.AptoFisico,
                EsSocioActivo = cliente.EsSocioActivo,
                FechaAlta = DateTime.UtcNow
            };
        }

        public ClienteResponse CreateClienteResponse(Cliente cliente)
        {
            return new ClienteResponse
            {                
                Id = cliente.Id,
                Dni = cliente.Dni,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Genero = cliente.Genero,
                FechaNacimiento = cliente.FechaNacimiento,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                Localidad = cliente.Localidad,
                CodigoPostal = cliente.CodigoPostal,
                Provincia = cliente.Provincia,
                Pais = cliente.Pais,
                ContactoEmergencia = cliente.ContactoEmergencia,
                Email = cliente.Email,
                ObraSocial = cliente.ObraSocial,
                AptoFisico = cliente.AptoFisico,
                EsSocioActivo = cliente.EsSocioActivo,
                FechaAlta = cliente.FechaAlta,
                FechaBaja = cliente.FechaBaja
            };
        }
    }
}
