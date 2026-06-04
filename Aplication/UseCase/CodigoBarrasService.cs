using Aplication.Interfaces;
using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.ICliente;
using Domain.Exceptions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace Aplication.UseCase
{
    public class CodigoBarrasService : ICodigoBarrasService
    {
        private readonly IAsistenciaQuery _asistenciaQuery;
        private readonly IClientesQuery _clientesQuery;

        public CodigoBarrasService(IAsistenciaQuery asistenciaQuery, IClientesQuery clientesQuery)
        {
            _asistenciaQuery = asistenciaQuery;
            _clientesQuery = clientesQuery;
        }

        public async Task<byte[]> GenerarPulseraPdfParaActividadAsync(int actividadId, int clienteId, bool esClase)
        {
            string codigoBarras;
            string nombreActividad;
            string fechaActividad;

            var cliente = await _clientesQuery.GetClienteById(clienteId);
            if (cliente == null) throw new ExceptionNotFound("Cliente no encontrado.");
            var nombreUsuario = $"{cliente.Nombre} {cliente.Apellido}";

            if (esClase)
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionClaseAsync(actividadId, clienteId);
                if (inscripcion == null) throw new ExceptionNotFound("Inscripción no encontrada en la clase.");

                codigoBarras = inscripcion.CodigoBarras ?? $"{cliente.Dni}{DateTime.UtcNow.Year}";
                // La entidad Asistencia tiene navegación a Clase
                nombreActividad = inscripcion.Clase?.Nombre ?? $"Clase #{actividadId}";
                fechaActividad = (inscripcion.Clase?.Fecha ?? DateTime.UtcNow).ToString("dd/MM/yyyy");
            }
            else
            {
                var inscripcion = await _asistenciaQuery.GetInscripcionEntrenamientoAsync(actividadId, clienteId);
                if (inscripcion == null) throw new ExceptionNotFound("Inscripción no encontrada en el entrenamiento.");

                codigoBarras = inscripcion.CodigoBarras ?? $"{cliente.Dni}{DateTime.UtcNow.Year}";
                nombreActividad = inscripcion.Entrenamiento?.Nombre ?? $"Entrenamiento #{actividadId}";
                fechaActividad = (inscripcion.Entrenamiento?.Fecha ?? DateTime.UtcNow).ToString("dd/MM/yyyy");
            }

            return await GenerarPulseraPdfAsync(codigoBarras, nombreUsuario, nombreActividad, fechaActividad);
        }

        public Task<byte[]> GenerarPulseraPdfAsync(string codigoBarras, string nombreUsuario, string nombreActividad, string fechaActividad)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // Generar imagen del código de barras Code-128
            var barcodeWriter = new BarcodeWriterGeneric
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions { Height = 100, Width = 300, Margin = 10 }
            };
            var bitMatrix = barcodeWriter.Encode(codigoBarras);

            // Convertir BitMatrix a PNG con SkiaSharp
            byte[] barcodeImageBytes;
            using (var bitmap = new SKBitmap(bitMatrix.Width, bitMatrix.Height))
            {
                for (int y = 0; y < bitMatrix.Height; y++)
                    for (int x = 0; x < bitMatrix.Width; x++)
                        bitmap.SetPixel(x, y, bitMatrix[x, y] ? SKColors.Black : SKColors.White);

                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                barcodeImageBytes = data.ToArray();
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(new PageSize(250, 70, Unit.Millimetre));
                    page.Margin(5, Unit.Millimetre);
                    page.PageColor(Colors.White);

                    page.Content().Row(row =>
                    {
                        row.RelativeItem().PaddingRight(10).Column(col =>
                        {
                            col.Item().Text("GOL AHORA - ACCESO").SemiBold().FontSize(14).FontColor(Colors.Blue.Darken2);
                            col.Item().Text($"Usuario: {nombreUsuario}").FontSize(10);
                            col.Item().Text($"Actividad: {nombreActividad}").FontSize(10);
                            col.Item().Text($"Fecha: {fechaActividad}").FontSize(10);
                        });

                        row.ConstantItem(120).AlignMiddle().AlignCenter().Column(col =>
                        {
                            col.Item().Image(barcodeImageBytes);
                            col.Item().Text(codigoBarras).FontSize(8).AlignCenter();
                        });
                    });
                });
            });

            return Task.FromResult(document.GeneratePdf());
        }
    }
}
