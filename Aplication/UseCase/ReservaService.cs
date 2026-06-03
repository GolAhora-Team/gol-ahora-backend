using Aplication.DTOs.Request.Reserva;
using Aplication.DTOs.Response.Reserva;
using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IReserva;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaQuery _reservaQuery;
        private readonly ICanchaQuery _canchaQuery;
        private readonly IClientesQuery _clienteQuery;
        private readonly IReservaCommand _reservaCommand;
        private readonly INotificacionService _notificacionService;

        public ReservaService(IReservaQuery reservaQuery, IReservaCommand reservaCommand, ICanchaQuery canchaQuery, IClientesQuery clienteQuery, INotificacionService notificacionService)
        {
            _reservaQuery = reservaQuery;
            _reservaCommand = reservaCommand;
            _canchaQuery = canchaQuery;
            _clienteQuery = clienteQuery;
            _notificacionService = notificacionService;
        }

        public async Task<CreateReservaResponse> CrearReserva(CreateReservaRequest request)
        {
            // Validar disponibilidad de la cancha
            var hasConflict = await _reservaQuery.ExisteReservaEnHorario(request.CanchaId, request.Fecha, request.HoraInicio, request.HoraFin);

            // validar la existencia del cliente y la cancha
            var clienteExists = await _clienteQuery.ClienteExists(request.ClienteId);
            if (!clienteExists)
            {
                throw new ExceptionBadRequest("El cliente no existe.");
            }
            var canchaExists = await _canchaQuery.CanchaExists(request.CanchaId);
            if (!canchaExists)
            {
                throw new ExceptionBadRequest("La cancha no existe.");
            }

            if (hasConflict)
            {
                throw new ExceptionBadRequest("La cancha no está disponible en el horario seleccionado.");
            }
            if (request.HoraInicio >= request.HoraFin)
            {
                throw new ExceptionBadRequest("La hora de inicio debe ser menor que la hora de fin.");
            }
            if (request.Fecha.Date < DateTime.Now.Date)
            {
                throw new ExceptionBadRequest("La fecha de la reserva no puede ser en el pasado.");
            }

            var reserva = new Reserva
            {
                Fecha = request.Fecha,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Estado = EstadoReserva.Confirmada,
                ClienteId = request.ClienteId,
                CanchaId = request.CanchaId
            };

            // Crear la reserva
            await _reservaCommand.InsertReserva(reserva);

            var cliente = await _clienteQuery.GetClienteById(request.ClienteId);
            await _notificacionService.CrearNotificacionGeneral(
                $"Reserva confirmada por {cliente?.Nombre} {cliente?.Apellido} para la cancha ID {reserva.CanchaId} el {reserva.Fecha:dd/MM/yyyy}.", 
                "ADMIN,PERSONAL", 
                "Reserva"
            );

            return new CreateReservaResponse
            {
                Id = reserva.Id,
                Fecha = request.Fecha,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Estado = "Pendiente",
                ClienteId = request.ClienteId,
                CanchaId = request.CanchaId
            };
        }

        public async Task<List<ReservaResponse>> GetAllReservas()
        {
            var reservas = await _reservaQuery.GetAllReservas();
            return reservas.Select(r => new ReservaResponse
            {
                Id = r.Id,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                Estado = r.Estado.ToString(),
                Cliente = new ClienteShort
                {
                    Id = r.Cliente.Id,
                    Nombre = r.Cliente.Nombre,
                    apellido = r.Cliente.Apellido
                },
                Cancha = new CanchaShort
                {
                    Id = r.Cancha.Id,
                    Nombre = r.Cancha.Nombre,
                    Tipo = r.Cancha.Tipo.ToString(),
                    Capacidad = r.Cancha.Capacidad
                }
            }).ToList();
        }

        public async Task<ReservaResponse> GetReservaById(int id)
        {
            var reserva = await _reservaQuery.GetReservaById(id);
            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada.");
            }
            return new ReservaResponse
            {
                Id = reserva.Id,
                Fecha = reserva.Fecha,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                Estado = reserva.Estado.ToString(),
                Cliente = new ClienteShort
                {
                    Id = reserva.Cliente.Id,
                    Nombre = reserva.Cliente.Nombre,
                    apellido = reserva.Cliente.Apellido
                },
                Cancha = new CanchaShort
                {
                    Id = reserva.Cancha.Id,
                    Nombre = reserva.Cancha.Nombre,
                    Tipo = reserva.Cancha.Tipo.ToString(),
                    Capacidad = reserva.Cancha.Capacidad
                }
            };
        }

        public async Task<ReservaResponse> CancelarReserva(int id)
        {
            var reserva = await _reservaQuery.GetReservaById(id);
            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada.");
            }
            if (reserva.Estado == EstadoReserva.Cancelada)
            {
                throw new ExceptionBadRequest("La reserva ya está cancelada.");
            }
            reserva.Estado = EstadoReserva.Cancelada;
            await _reservaCommand.UpdateReserva(reserva);
            return new ReservaResponse
            {
                Id = reserva.Id,
                Fecha = reserva.Fecha,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                Estado = reserva.Estado.ToString(),
                Cliente = new ClienteShort
                {
                    Id = reserva.Cliente.Id,
                    Nombre = reserva.Cliente.Nombre,
                    apellido = reserva.Cliente.Apellido
                },
                Cancha = new CanchaShort
                {
                    Id = reserva.Cancha.Id,
                    Nombre = reserva.Cancha.Nombre,
                    Tipo = reserva.Cancha.Tipo.ToString(),
                    Capacidad = reserva.Cancha.Capacidad
                }
            };
        }

        public async Task<ReservaResponse> ModificarReserva(int id, UpdateReservaRequest request)
        {
            //verifico reserva existe y no esta cancelada
            var reserva = await _reservaQuery.GetReservaById(id);
            if (reserva == null)
            {
                throw new ExceptionNotFound("Reserva no encontrada.");
            }
            if (reserva.Estado == EstadoReserva.Cancelada)
            {
                throw new ExceptionBadRequest("No se puede modificar una reserva cancelada.");
            }

            // Validar disponibilidad de la cancha
            var hasConflict = await _reservaQuery.ExisteReservaEnHorario(request.CanchaId, request.Fecha, request.HoraInicio, request.HoraFin, id);
            if (hasConflict)
            {
                throw new ExceptionBadRequest("La cancha no está disponible en el horario seleccionado.");
            }
            if (request.HoraInicio >= request.HoraFin)
            {
                throw new ExceptionBadRequest("La hora de inicio debe ser menor que la hora de fin.");
            }
            if (request.Fecha.Date < DateTime.Now.Date)
            {
                throw new ExceptionBadRequest("La fecha de la reserva no puede ser en el pasado.");
            }

            reserva.Fecha = request.Fecha;
            reserva.HoraInicio = request.HoraInicio;
            reserva.HoraFin = request.HoraFin;
            reserva.CanchaId = request.CanchaId;
            reserva.Cancha = await _canchaQuery.GetCanchaById(request.CanchaId);
            await _reservaCommand.UpdateReserva(reserva);

            return new ReservaResponse
            {
                Id = reserva.Id,
                Fecha = reserva.Fecha,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                Estado = reserva.Estado.ToString(),
                Cliente = new ClienteShort
                {
                    Id = reserva.Cliente.Id,
                    Nombre = reserva.Cliente.Nombre,
                    apellido = reserva.Cliente.Apellido
                },
                Cancha = new CanchaShort
                {
                    Id = reserva.Cancha.Id,
                    Nombre = reserva.Cancha.Nombre,
                    Tipo = reserva.Cancha.Tipo.ToString(),
                    Capacidad = reserva.Cancha.Capacidad
                }
            };
        }
    }
}
