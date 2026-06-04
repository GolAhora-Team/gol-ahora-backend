using Aplication.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.Port,
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
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
