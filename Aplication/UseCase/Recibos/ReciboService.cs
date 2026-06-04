using Aplication.Interfaces.IRecibo;
using Aplication.Interfaces.IReserva;
using Domain.Entities;
using Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aplication.UseCase.Recibos
{
    public class ReciboService : IReciboService
    {
        private readonly IReciboQuery _reciboQuery;
        private readonly IReciboCommand _reciboCommand;
        private readonly IReservaQuery _reservaQuery;

        public ReciboService(IReciboQuery reciboQuery, IReciboCommand reciboCommand, IReservaQuery reservaQuery)
        {
            _reciboQuery = reciboQuery;
            _reciboCommand = reciboCommand;
            _reservaQuery = reservaQuery;
            
            // Requerido por QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerarPdfRecibo(int reservaId)
        {
            var reserva = await _reservaQuery.GetReservaById(reservaId);
            if (reserva == null)
            {
                throw new Exception("Reserva no encontrada");
            }

            if (reserva.Estado != EstadoReserva.Confirmada)
            {
                throw new Exception("La reserva debe estar pagada o confirmada para emitir un recibo.");
            }

            var recibo = await _reciboQuery.GetReciboByReservaId(reservaId);

            if (recibo == null)
            {
                // Generar nuevo recibo
                int lastId = await _reciboQuery.GetLastReciboId();
                int nextId = lastId + 1;
                string numeroRecibo = $"0001-{nextId:D8}";

                // Asumimos un monto por defecto ya que la tabla de Pagos puede ser compleja de enlazar aquí
                // O idealmente lo sacaríamos de PrecioCancha, pero por ahora tomaremos un valor de la reserva o harcodeado
                // El precio no está directo en Cancha, habría que obtenerlo, pero podemos poner 0 temporalmente
                // o extraer de una tabla Precios si es necesario.
                decimal montoTotal = 0; // Se debe calcular el total real si Cancha no tiene Precio.

                recibo = new Recibo
                {
                    ReservaId = reservaId,
                    NumeroRecibo = numeroRecibo,
                    FechaEmision = DateTime.Now,
                    MontoTotal = montoTotal,
                    Concepto = $"Pago total por reserva de cancha de {reserva.Cancha?.Superficie} - Turno: {reserva.Fecha:dd/MM/yyyy} {reserva.HoraInicio:hh\\:mm}hs",
                    MetodoPago = "Transferencia", // Asumido por defecto si no tenemos enlace directo
                    FirmaDigital = "GOL AHORA"
                };

                await _reciboCommand.InsertRecibo(recibo);
            }

            // Generar PDF usando QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(compose => ComposeHeader(compose, recibo));
                    page.Content().Element(compose => ComposeContent(compose, recibo));
                    page.Footer().Element(compose => ComposeFooter(compose, recibo));
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, Recibo recibo)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("GOL AHORA").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text("Complejo Deportivo");
                    column.Item().Text("Av. Siempre Viva 123, Springfield");
                });

                row.ConstantItem(150).AlignRight().Column(column =>
                {
                    column.Item().Text($"Recibo N°: {recibo.NumeroRecibo}").Bold();
                    column.Item().Text($"Fecha: {recibo.FechaEmision:dd/MM/yyyy HH:mm}");
                });
            });
        }

        private void ComposeContent(IContainer container, Recibo recibo)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Item().Text("DATOS DEL CLIENTE").SemiBold().FontSize(14).Underline();
                column.Item().PaddingBottom(10).Text(text =>
                {
                    text.Span("Nombre Completo: ").SemiBold();
                    text.Span($"{recibo.Reserva.Cliente?.Nombre} {recibo.Reserva.Cliente?.Apellido}");
                    text.Line("");
                    text.Span("DNI: ").SemiBold();
                    text.Span($"{(recibo.Reserva.Cliente != null ? recibo.Reserva.Cliente.Dni.ToString() : "N/A")}");
                });

                column.Item().Text("DETALLE DE LA OPERACIÓN").SemiBold().FontSize(14).Underline();
                
                column.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(5).Text("Concepto").SemiBold();
                        header.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(5).AlignRight().Text("Medio de Pago").SemiBold();
                        header.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(5).AlignRight().Text("Importe").SemiBold();
                    });

                    table.Cell().PaddingTop(5).Text(recibo.Concepto);
                    table.Cell().PaddingTop(5).AlignRight().Text(recibo.MetodoPago);
                    table.Cell().PaddingTop(5).AlignRight().Text($"${recibo.MontoTotal:N2}");
                });

                column.Item().PaddingTop(25).AlignRight().Text(text =>
                {
                    text.Span("TOTAL ABONADO: ").SemiBold().FontSize(14);
                    text.Span($"${recibo.MontoTotal:N2}").Bold().FontSize(14).FontColor(Colors.Green.Darken2);
                });
            });
        }

        private void ComposeFooter(IContainer container, Recibo recibo)
        {
            container.AlignCenter().Column(column =>
            {
                column.Item().Text("_________________________________");
                column.Item().Text(recibo.FirmaDigital).Bold().FontSize(14);
                column.Item().Text("Firma Administración");
            });
        }
    }
}
