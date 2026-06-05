using Aplication.DTOs.Request.Reserva;
using Aplication.DTOs.Response.Reserva;
using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IReserva;
using Aplication.Interfaces.IPago;
using Aplication.Interfaces.IFactura;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.Interfaces.IDescuento;
using Aplication.Interfaces.IUsuario;
using Aplication.Interfaces.IConfiguracion;

namespace Aplication.UseCase
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaQuery _reservaQuery;
        private readonly ICanchaQuery _canchaQuery;
        private readonly IClientesQuery _clienteQuery;
        private readonly IReservaCommand _reservaCommand;
        private readonly INotificacionService _notificacionService;
        private readonly IPagoCommand _pagoCommand;
        private readonly IFacturaQuery _facturaQuery;
        private readonly IPagoQuery _pagoQuery;
        private readonly IDescuentoCommand _descuentoCommand;
        private readonly IUsuarioQuery _usuarioQuery;
        private readonly IConfiguracionQuery _configuracionQuery;

        public ReservaService(
            IReservaQuery reservaQuery,
            IReservaCommand reservaCommand,
            ICanchaQuery canchaQuery,
            IClientesQuery clienteQuery,
            INotificacionService notificacionService,
            IPagoCommand pagoCommand,
            IFacturaQuery facturaQuery,
            IPagoQuery pagoQuery,
            IDescuentoCommand descuentoCommand,
            IUsuarioQuery usuarioQuery,
            IConfiguracionQuery configuracionQuery)
        {
            _reservaQuery = reservaQuery;
            _reservaCommand = reservaCommand;
            _canchaQuery = canchaQuery;
            _clienteQuery = clienteQuery;
            _notificacionService = notificacionService;
            _pagoCommand = pagoCommand;
            _facturaQuery = facturaQuery;
            _pagoQuery = pagoQuery;
            _descuentoCommand = descuentoCommand;
            _usuarioQuery = usuarioQuery;
            _configuracionQuery = configuracionQuery;
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
            if (request.Fecha.Date > DateTime.Now.Date.AddDays(30))
            {
                throw new ExceptionBadRequest("No se puede reservar con más de 30 días de antelación.");
            }

            var reserva = new Reserva
            {
                Fecha = request.Fecha,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Estado = request.Estado ?? EstadoReserva.Pendiente,
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

        // ==========================================
        // 🔥 NUEVO: Obtener info de cancelación con cálculo de penalización
        // ==========================================
        public async Task<CancelacionInfoResponse> GetCancelacionInfo(int reservaId)
        {
            var reserva = await _reservaQuery.GetReservaById(reservaId);
            if (reserva == null)
                throw new ExceptionNotFound("Reserva no encontrada.");

            if (reserva.Estado == EstadoReserva.Cancelada)
                throw new ExceptionBadRequest("La reserva ya está cancelada.");

            if (reserva.Estado == EstadoReserva.Finalizada)
                throw new ExceptionBadRequest("La reserva ya finalizó.");

            var conf = await _configuracionQuery.GetConfiguracion();
            int horasAntelacion = conf.HorasAntelacionMinima;

            // Calcular horas restantes hasta el turno
            var fechaHoraTurno = reserva.Fecha.Date + reserva.HoraInicio;
            var horasRestantes = (fechaHoraTurno - DateTime.Now).TotalHours;

            // Buscar monto original pagado
            decimal montoOriginal = 0;
            int? facturaIdEncontrada = reserva.FacturaId;

            if (reserva.FacturaId.HasValue)
            {
                var factura = await _facturaQuery.GetFacturaById(reserva.FacturaId.Value);
                if (factura != null)
                {
                    decimal sumaPagos = factura.Pagos?
                        .Where(p => p.Estado == EstadoPago.Pagado)
                        .Sum(p => p.Monto) ?? 0;
                    montoOriginal = sumaPagos > 0 ? sumaPagos : factura.Total;
                }
            }
            else
            {
                // Buscar factura por clienteId + concepto "Reserva"
                var facturas = await _facturaQuery.GetFacturasByClienteId(reserva.ClienteId);
                var factura = facturas?
                    .Where(f => f.Concepto == "Reserva")
                    .OrderByDescending(f => f.FechaEmision)
                    .FirstOrDefault();
                if (factura != null)
                {
                    facturaIdEncontrada = factura.Id;
                    decimal sumaPagos = factura.Pagos?
                        .Where(p => p.Estado == EstadoPago.Pagado)
                        .Sum(p => p.Monto) ?? 0;
                    montoOriginal = sumaPagos > 0 ? sumaPagos : factura.Total;
                }
            }

            bool dentroDePlazo = horasRestantes >= horasAntelacion;
            decimal penalizacionAplicable = 0;

            if (!dentroDePlazo)
            {
                penalizacionAplicable = conf.PorcentajePenalizacion;
            }

            decimal montoPenalizacion = montoOriginal * (penalizacionAplicable / 100m);
            decimal montoReintegro = montoOriginal - montoPenalizacion;

            return new CancelacionInfoResponse
            {
                ReservaId = reserva.Id,
                FechaReserva = reserva.Fecha,
                HoraInicio = reserva.HoraInicio,
                ClienteNombre = $"{reserva.Cliente?.Nombre} {reserva.Cliente?.Apellido}",
                CanchaNombre = reserva.Cancha?.Nombre ?? "N/A",
                MontoOriginal = montoOriginal,
                HorasRestantes = Math.Round(horasRestantes, 1),
                HorasAntelacionMinima = horasAntelacion,
                DentroDePlazo = dentroDePlazo,
                PorcentajePenalizacion = penalizacionAplicable,
                MontoPenalizacion = montoPenalizacion,
                MontoReintegro = montoReintegro
            };
        }

        // ==========================================
        // 🔥 MODIFICADO: Cancelar reserva con reintegro financiero
        // ==========================================
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

            // Obtener info de cancelación (cálculo de penalización)
            var cancelInfo = await GetCancelacionInfo(id);

            // 1. Marcar reserva como cancelada
            reserva.Estado = EstadoReserva.Cancelada;
            await _reservaCommand.UpdateReserva(reserva);

            // 2. Si hay monto a reintegrar, generar el pago negativo (nota de crédito)
            if (cancelInfo.MontoReintegro > 0)
            {
                if (reserva.Cliente != null && reserva.Cliente.EsSocioActivo && cancelInfo.DentroDePlazo)
                {
                    // Es socio activo y canceló en término: crear cupón de descuento 100%
                    var descuento = new Descuento
                    {
                        Nombre = $"REFUND-{reserva.Id}",
                        Descripcion = $"Reembolso de reserva {reserva.Id} (100% off)",
                        Porcentaje = 100,
                        FechaInicio = DateTime.Now,
                        FechaFin = DateTime.Now.AddMonths(3)
                    };
                    await _descuentoCommand.InsertDescuento(descuento);

                    var usuario = await _usuarioQuery.GetUsuarioByPersonaId(reserva.ClienteId);
                    if (usuario != null)
                    {
                        await _notificacionService.CrearNotificacionUsuario(
                            $"Reembolso procesado. Tienes un descuento del 100% para tu próxima reserva usando el código: REFUND-{reserva.Id}",
                            usuario.Id,
                            "Reembolso"
                        );
                    }
                }
                else
                {
                    // Buscar la factura asociada
                    int? facturaId = reserva.FacturaId;
                    if (!facturaId.HasValue)
                    {
                        var facturas = await _facturaQuery.GetFacturasByClienteId(reserva.ClienteId);
                        var factura = facturas?
                            .Where(f => f.Concepto == "Reserva")
                            .OrderByDescending(f => f.FechaEmision)
                            .FirstOrDefault();
                        facturaId = factura?.Id;
                    }

                    if (facturaId.HasValue)
                    {
                        var pagoReintegro = new Pago
                        {
                            FechaPago = DateTime.Now,
                            Monto = -cancelInfo.MontoReintegro, // Monto negativo = nota de crédito/reintegro
                            Metodo = MetodoPago.Transferencia,
                            Estado = EstadoPago.Reintegro,
                            FacturaId = facturaId.Value
                        };
                        await _pagoCommand.InsertPago(pagoReintegro);
                    }
                }
            }

            // 3. Notificar
            var cliente = reserva.Cliente;
            string mensajePenalizacion = cancelInfo.DentroDePlazo 
                ? "Reintegro total." 
                : $"Penalización del {cancelInfo.PorcentajePenalizacion}% aplicada. Reintegro: ${cancelInfo.MontoReintegro}";

            await _notificacionService.CrearNotificacionGeneral(
                $"Reserva cancelada: {cliente?.Nombre} {cliente?.Apellido} - Cancha {reserva.Cancha?.Nombre} - {reserva.Fecha:dd/MM/yyyy}. {mensajePenalizacion}",
                "ADMIN,PERSONAL",
                "Cancelacion"
            );

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
            if (request.Fecha.Date > DateTime.Now.Date.AddDays(30))
            {
                throw new ExceptionBadRequest("No se puede reservar con más de 30 días de antelación.");
            }

            if (request.Estado.HasValue && request.Estado.Value == EstadoReserva.Confirmada && reserva.Estado != EstadoReserva.Confirmada)
            {
                int facturaToCheck = request.FacturaId ?? reserva.FacturaId ?? 0;
                if (facturaToCheck == 0)
                {
                    throw new ExceptionBadRequest("El pago completo debe ser registrado y validado para confirmar la reserva.");
                }
                var factura = await _facturaQuery.GetFacturaById(facturaToCheck);
                if (factura == null)
                {
                    throw new ExceptionBadRequest("Factura no encontrada para confirmar la reserva.");
                }
                decimal totalPagado = factura.Pagos?.Where(p => p.Estado == EstadoPago.Pagado).Sum(p => p.Monto) ?? 0;
                if (totalPagado < factura.Total)
                {
                    throw new ExceptionBadRequest("El pago completo debe ser registrado y validado para confirmar la reserva.");
                }
            }

            reserva.Fecha = request.Fecha;
            reserva.HoraInicio = request.HoraInicio;
            reserva.HoraFin = request.HoraFin;
            reserva.CanchaId = request.CanchaId;
            reserva.Cancha = await _canchaQuery.GetCanchaById(request.CanchaId);
            
            if (request.FacturaId.HasValue)
            {
                reserva.FacturaId = request.FacturaId.Value;
            }
            if (request.Estado.HasValue)
            {
                reserva.Estado = request.Estado.Value;
            }

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
