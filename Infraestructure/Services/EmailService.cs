using Aplication.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("complejogolahora@gmail.com", "fdfu uehq fwhq gqxa"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("complejogolahora@gmail.com", "Complejo Gol Ahora"),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (FormatException ex)
            {
                throw new Exception("El formato del correo electrónico es inválido.", ex);
            }
            catch (SmtpFailedRecipientException ex)
            {
                throw new Exception("La dirección de correo electrónico del destinatario no existe o no es válida.", ex);
            }
            catch (SmtpException ex)
            {
                throw new Exception("Hubo un problema de red o autenticación al intentar enviar el correo. Por favor, intenta de nuevo más tarde.", ex);
            }
        }
    }
}
