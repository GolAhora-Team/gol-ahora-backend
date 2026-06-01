using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IEquipo;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class EquipoService : IEquipoService
    {
        private readonly IEquipoCommand _command;
        private readonly IEquipoQuery _query;
        private readonly IEquipoMapper _mapper;

        public EquipoService(IEquipoCommand command, IEquipoQuery query, IEquipoMapper mapper)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
        }

        public async Task<EquipoResponse> CreateEquipo(CreateEquipoRequest request)
        {
            var equipo = _mapper.CreateEquipo(request);

            await _command.InsertEquipo(equipo);

            equipo = await _query.GetEquipoById(equipo.Id);
            return _mapper.CreateEquipoResponse(equipo);
        }

        public async Task<EquipoResponse> DeleteEquipo(int equipoId)
        {
            var cliente = await _query.GetEquipoById(equipoId);
            if (cliente == null)
            {
                throw new Exception("Equipo no encontrado");
            }

            await _command.RemoveEquipo(cliente.Id);
            return _mapper.CreateEquipoResponse(cliente);
        }

        public async Task<List<EquipoResponse>> GetAll()
        {
            var equipos = await _query.GetListEquipos();

            return equipos.Select(equipo => _mapper.CreateEquipoResponse(equipo)).ToList();
        }

        public async Task<EquipoResponse> GetEquipoById(int equipoId)
        {
            var equipo = await _query.GetEquipoById(equipoId);

            if (equipo == null)
                throw new Exception("El equipo no existe");

            return _mapper.CreateEquipoResponse(equipo);
        }

        public async Task<EquipoResponse> UpdateEquipo(int equipoId, CreateEquipoRequest request)
        {
            var equipoOriginal = await _query.GetEquipoById(equipoId);

            if (equipoOriginal == null)
                throw new Exception("El equipo no existe");

            equipoOriginal.Nombre = request.Nombre;
            equipoOriginal.CantidadMaxJugadores = request.CantidadMaxJugadores;
            equipoOriginal.Descripcion = request.Descripcion;
            equipoOriginal.ColorPrimario = request.ColorPrimario ?? equipoOriginal.ColorPrimario;
            equipoOriginal.ColorSecundario = request.ColorSecundario ?? equipoOriginal.ColorSecundario;
            equipoOriginal.CompeticionId = request.CompeticionId;

            await _command.UpdateEquipo(equipoOriginal);

            equipoOriginal = await _query.GetEquipoById(equipoOriginal.Id);
            return _mapper.CreateEquipoResponse(equipoOriginal);
        }

        
    }
}
