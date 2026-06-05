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

        public async Task<List<AsistenciaResponse>> GetAsistenciasPorActividadYFecha(int actividadId, DateTime fecha, bool esClase)
        {
            if (esClase)
            {
                var asistencias = await _asistenciaQuery.GetAsistenciasPorClaseYFecha(actividadId, fecha);
                return _asistenciaMapper.MapToResponseList(asistencias);
            }
            else
            {
                var asistenciasEntrenamiento = await _asistenciaQuery.GetAsistenciasEntrenamientoPorFecha(actividadId, fecha);
                var responses = new List<AsistenciaResponse>();
                foreach (var a in asistenciasEntrenamiento)
                {
                    responses.Add(new AsistenciaResponse
                    {
                        Id = a.Id,
                        ClienteId = a.ClienteId,
                        Presente = a.Presente,
                        Fecha = a.Fecha,
                        ClaseId = a.EntrenamientoId
                    });
                }
                return responses;
            }
        }

        public async Task<List<AsistenciaResponse>> MarcarAsistencia(MarcarAsistenciaRequest request)
        {
            throw new Exception("Metodo deprecado. Utilizar RegistrarAsistenciaManual.");
        }

        public async Task<bool> RegistrarAsistenciaManual(int actividadId, int clienteId, bool esClase)
        {
            return await RegistrarAsistenciaBase(actividadId, clienteId, esClase, "Manual");
        }

        public async Task<bool> RegistrarAsistenciaCodigoBarras(string codigoBarras, int actividadId, bool esClase)
        {
            int clienteId = 0;
            if (esClase)
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionClaseByBarcodeAsync(actividadId, codigoBarras);
                if (inscripcion == null)
                    throw new ExceptionNotFound("No se encontró inscripción para este código de barras en esta clase.");
                clienteId = inscripcion.ClienteId;
            }
            else
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionEntrenamientoByBarcodeAsync(actividadId, codigoBarras);
                if (inscripcion == null)
                    throw new ExceptionNotFound("No se encontró inscripción para este código de barras en este entrenamiento.");
                clienteId = inscripcion.ClienteId;
            }

            return await RegistrarAsistenciaBase(actividadId, clienteId, esClase, "CodigoBarras");
        }

        private async Task<bool> RegistrarAsistenciaBase(int actividadId, int clienteId, bool esClase, string metodo)
        {
            var hoy = DateTime.UtcNow.Date;

            if (esClase)
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionClaseAsync(actividadId, clienteId);
                
                if (inscripcion == null)
                    throw new ExceptionNotFound("El cliente no está inscrito en esta clase.");

                if (inscripcion.Presente && inscripcion.FechaHoraRegistro?.Date == hoy)
                {
                    throw new ExceptionBadRequest("La asistencia ya fue registrada para este usuario en el día de hoy.");
                }

                inscripcion.Presente = true;
                inscripcion.FechaHoraRegistro = DateTime.UtcNow;
                inscripcion.MetodoRegistro = metodo;
                
                await _asistenciaCommand.UpdateAsistencia(inscripcion);
                return true;
            }
            else
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionEntrenamientoAsync(actividadId, clienteId);

                if (inscripcion == null)
                    throw new ExceptionNotFound("El cliente no está inscrito en este entrenamiento.");

                var yaAsistio = await _asistenciaQuery.YaAsistioEntrenamientoAsync(actividadId, clienteId, hoy);

                if (yaAsistio)
                {
                    throw new ExceptionBadRequest("La asistencia ya fue registrada para este usuario en el día de hoy.");
                }

                var nuevaAsistencia = new AsistenciaEntrenamiento
                {
                    EntrenamientoId = actividadId,
                    ClienteId = clienteId,
                    Presente = true,
                    Fecha = hoy,
                    FechaHoraRegistro = DateTime.UtcNow,
                    MetodoRegistro = metodo,
                    CodigoBarras = inscripcion.CodigoBarras
                };

                await _asistenciaCommand.InsertAsistenciaEntrenamiento(nuevaAsistencia);
                return true;
            }
        }
    }
}
