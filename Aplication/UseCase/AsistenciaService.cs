using Aplication.DTOs.Request.Asistencia;
using Aplication.DTOs.Response.Asistencia;
using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.IClases;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaCommand _asistenciaCommand;
        private readonly IAsistenciaQuery _asistenciaQuery;
        private readonly IAsistenciaMapper _asistenciaMapper;
        private readonly IClaseQuery _claseQuery;

        public AsistenciaService(
            IAsistenciaCommand asistenciaCommand, 
            IAsistenciaQuery asistenciaQuery, 
            IAsistenciaMapper asistenciaMapper,
            IClaseQuery claseQuery)
        {
            _asistenciaCommand = asistenciaCommand;
            _asistenciaQuery = asistenciaQuery;
            _asistenciaMapper = asistenciaMapper;
            _claseQuery = claseQuery;
        }

        public async Task<List<AsistenciaResponse>> GetAsistenciasPorClaseYFecha(int claseId, DateTime fecha)
        {
            var asistencias = await _asistenciaQuery.GetAsistenciasPorClaseYFecha(claseId, fecha);
            return _asistenciaMapper.MapToResponseList(asistencias);
        }

        public async Task<List<AsistenciaResponse>> MarcarAsistencia(MarcarAsistenciaRequest request)
        {
            var clase = await _claseQuery.GetClaseById(request.ClaseId);
            if (clase == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }

            var asistenciasExistentes = await _asistenciaQuery.GetAsistenciasPorClaseYFecha(request.ClaseId, request.Fecha.Date);

            // Create a lookup for existing attendance records by ClienteId
            var existentesDict = asistenciasExistentes.ToDictionary(a => a.ClienteId, a => a);
            
            var responses = new List<AsistenciaResponse>();

            // Asumimos que los clientes en la lista de presentes deben marcarse como Presente=true.
            // Si el cliente no está en la lista pero pertenece a la clase, se podría marcar como Presente=false.
            // Para simplificar, marcaremos los proporcionados como presentes.
            // Lo ideal sería obtener los Clientes asociados a la Clase/Entrenamiento para marcar ausentes a los que no están en la lista.
            
            // Marcar todos los enviados en la request
            foreach (var clienteId in request.ClientesPresentesIds)
            {
                if (existentesDict.TryGetValue(clienteId, out var asistencia))
                {
                    // Update if already exists and was false
                    if (!asistencia.Presente)
                    {
                        asistencia.Presente = true;
                        await _asistenciaCommand.UpdateAsistencia(asistencia);
                    }
                    responses.Add(_asistenciaMapper.MapToResponse(asistencia));
                    // Remove from dict to know who is left
                    existentesDict.Remove(clienteId);
                }
                else
                {
                    // Create new record
                    var nuevaAsistencia = new Asistencia
                    {
                        ClaseId = request.ClaseId,
                        ClienteId = clienteId,
                        Fecha = request.Fecha.Date,
                        Presente = true
                    };
                    await _asistenciaCommand.InsertAsistencia(nuevaAsistencia);
                    responses.Add(_asistenciaMapper.MapToResponse(nuevaAsistencia));
                }
            }

            // The remaining in the dict should be marked as not present (if we want to toggle them back)
            foreach (var kvp in existentesDict)
            {
                if (kvp.Value.Presente)
                {
                    kvp.Value.Presente = false;
                    await _asistenciaCommand.UpdateAsistencia(kvp.Value);
                }
                responses.Add(_asistenciaMapper.MapToResponse(kvp.Value));
            }

            return responses;
        }
    }
}
