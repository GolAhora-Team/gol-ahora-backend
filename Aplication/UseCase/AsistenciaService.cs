using Aplication.DTOs.Request.Asistencia;
using Aplication.DTOs.Response.Asistencia;
using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.IEntrenamiento;
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
        private readonly IEntrenamientoQuery _entrenamientoQuery;

        public AsistenciaService(
            IAsistenciaCommand asistenciaCommand, 
            IAsistenciaQuery asistenciaQuery, 
            IAsistenciaMapper asistenciaMapper,
            IClaseQuery claseQuery,
            IEntrenamientoQuery entrenamientoQuery)
        {
            _asistenciaCommand = asistenciaCommand;
            _asistenciaQuery = asistenciaQuery;
            _asistenciaMapper = asistenciaMapper;
            _claseQuery = claseQuery;
            _entrenamientoQuery = entrenamientoQuery;
        }

        public async Task<List<AsistenciaResponse>> GetAsistenciasPorActividadYFecha(int actividadId, DateTime fecha, bool esClase)
        {
            if (esClase)
            {
                var asistencias = await _asistenciaQuery.GetAsistenciasPorClaseYFecha(actividadId, fecha);
                var responses = new List<AsistenciaResponse>();
                foreach (var a in asistencias)
                {
                    responses.Add(new AsistenciaResponse
                    {
                        Id = a.Id,
                        ClienteId = a.ClienteId,
                        Presente = a.Presente,
                        Fecha = a.Fecha,
                        ClaseId = a.ClaseId
                    });
                }
                return responses;
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

        public async Task<List<AsistenciaResponse>> GetHistorialAsistencias(int actividadId, int clienteId, bool esClase)
        {
            var responses = new List<AsistenciaResponse>();

            if (esClase)
            {
                var historial = await _asistenciaQuery.GetHistorialAsistenciaClase(actividadId, clienteId);
                foreach (var a in historial)
                {
                    responses.Add(new AsistenciaResponse
                    {
                        Id = a.Id,
                        ClienteId = a.ClienteId,
                        Presente = a.Presente,
                        Fecha = a.Fecha,
                        ClaseId = a.ClaseId
                    });
                }
            }
            else
            {
                var historial = await _asistenciaQuery.GetHistorialAsistenciaEntrenamiento(actividadId, clienteId);
                foreach (var a in historial)
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
            }

            return responses;
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

        private string GetDayAbbr(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return "Dom";
                case DayOfWeek.Monday: return "Lun";
                case DayOfWeek.Tuesday: return "Mar";
                case DayOfWeek.Wednesday: return "Mié";
                case DayOfWeek.Thursday: return "Jue";
                case DayOfWeek.Friday: return "Vie";
                case DayOfWeek.Saturday: return "Sáb";
                default: return "";
            }
        }

        public async Task<bool> EliminarAsistencia(int actividadId, int clienteId, bool esClase)
        {
            var hoy = DateTime.UtcNow.AddHours(-3).Date;

            if (esClase)
            {
                var registro = await _asistenciaQuery.GetRegistroAsistenciaClaseAsync(actividadId, clienteId, hoy);
                if (registro == null)
                    throw new ExceptionNotFound("No se encontró la asistencia marcada para hoy.");

                await _asistenciaCommand.RemoveRegistroAsistenciaClase(registro);

                // Actualizar la inscripción para reflejar que no está presente hoy
                var inscripcion = await _asistenciaQuery.GetInscripcionClaseAsync(actividadId, clienteId);
                if (inscripcion != null)
                {
                    inscripcion.Presente = false;
                    await _asistenciaCommand.UpdateAsistencia(inscripcion);
                }
            }
            else
            {
                var registro = await _asistenciaQuery.GetAsistenciaEntrenamientoAsync(actividadId, clienteId, hoy);
                if (registro == null)
                    throw new ExceptionNotFound("No se encontró la asistencia marcada para hoy.");

                await _asistenciaCommand.RemoveAsistenciaEntrenamiento(registro);
            }

            return true;
        }

        private async Task<bool> RegistrarAsistenciaBase(int actividadId, int clienteId, bool esClase, string metodo)
        {
            var now = DateTime.UtcNow.AddHours(-3);
            var hoy = now.Date;
            var diaAbbr = GetDayAbbr(now.DayOfWeek);

            if (esClase)
            {
                var clase = await _claseQuery.GetClaseById(actividadId);
                if (clase != null && !string.IsNullOrEmpty(clase.DiasSemana))
                {
                    var diasList = clase.DiasSemana.Split(',').Select(d => d.Trim()).ToList();
                    if (!diasList.Contains(diaAbbr))
                        throw new ExceptionBadRequest($"No se puede tomar asistencia hoy. La clase se dicta: {clase.DiasSemana}");
                }

                var inscripcion = await _asistenciaQuery.GetInscripcionClaseAsync(actividadId, clienteId);
                if (inscripcion == null)
                    throw new ExceptionNotFound("El cliente no está inscrito en esta clase.");

                var yaAsistio = await _asistenciaQuery.GetRegistroAsistenciaClaseAsync(actividadId, clienteId, hoy);
                if (yaAsistio != null)
                    throw new ExceptionBadRequest("La asistencia ya fue registrada para este usuario en el día de hoy.");

                var nuevoRegistro = new RegistroAsistenciaClase
                {
                    ClaseId = actividadId,
                    ClienteId = clienteId,
                    Presente = true,
                    Fecha = hoy,
                    FechaHoraRegistro = DateTime.UtcNow,
                    MetodoRegistro = metodo,
                    CodigoBarras = inscripcion.CodigoBarras
                };
                await _asistenciaCommand.InsertRegistroAsistenciaClase(nuevoRegistro);

                inscripcion.Presente = true;
                inscripcion.FechaHoraRegistro = DateTime.UtcNow;
                inscripcion.MetodoRegistro = metodo;
                await _asistenciaCommand.UpdateAsistencia(inscripcion);

                return true;
            }
            else
            {
                var entrenamiento = await _entrenamientoQuery.GetEntrenamientoById(actividadId);
                if (entrenamiento != null && !string.IsNullOrEmpty(entrenamiento.DiasSemana))
                {
                    var diasList = entrenamiento.DiasSemana.Split(',').Select(d => d.Trim()).ToList();
                    if (!diasList.Contains(diaAbbr))
                        throw new ExceptionBadRequest($"No se puede tomar asistencia hoy. El entrenamiento se dicta: {entrenamiento.DiasSemana}");
                }

                var inscripcion = await _asistenciaQuery.GetInscripcionEntrenamientoAsync(actividadId, clienteId);
                if (inscripcion == null)
                    throw new ExceptionNotFound("El cliente no está inscrito en este entrenamiento.");

                var yaAsistio = await _asistenciaQuery.YaAsistioEntrenamientoAsync(actividadId, clienteId, hoy);
                if (yaAsistio)
                    throw new ExceptionBadRequest("La asistencia ya fue registrada para este usuario en el día de hoy.");

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
