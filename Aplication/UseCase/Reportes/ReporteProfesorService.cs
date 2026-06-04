using Aplication.Interfaces;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading.Tasks;

namespace Aplication.UseCase.Reportes
{
    public interface IReporteProfesorService
    {
        Task<byte[]> GenerarReporteProfesorPdfAsync(Profesor profesor);
    }

    public class ReporteProfesorService : IReporteProfesorService
    {
        public Task<byte[]> GenerarReporteProfesorPdfAsync(Profesor profesor)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text($"Reporte de Profesor: {profesor.Nombre} {profesor.Apellido}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Text("Datos Personales").SemiBold().FontSize(16);
                        col.Item().Text($"DNI: {profesor.Dni}");
                        col.Item().Text($"Email: {profesor.Email}");
                        col.Item().Text($"Teléfono: {profesor.Telefono}");
                        
                        col.Item().PaddingTop(10).Text("Datos Profesionales").SemiBold().FontSize(16);
                        col.Item().Text($"Especialidad: {profesor.Especialidad}");
                        col.Item().Text($"Certificación: {profesor.Certificacion}");
                        
                        string validez = "No registra";
                        if (profesor.CertificadoFechaInicio.HasValue && profesor.CertificadoFechaFin.HasValue)
                        {
                            validez = $"{profesor.CertificadoFechaInicio.Value:dd/MM/yyyy} al {profesor.CertificadoFechaFin.Value:dd/MM/yyyy}";
                        }
                        col.Item().Text($"Validez Certificado: {validez}");
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            });

            return Task.FromResult(document.GeneratePdf());
        }
    }
}
