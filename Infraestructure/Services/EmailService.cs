using Aplication.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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
                var smtpServer = string.IsNullOrWhiteSpace(_emailSettings.SmtpServer) ? "smtp.gmail.com" : _emailSettings.SmtpServer;
                var port = _emailSettings.Port <= 0 ? 587 : _emailSettings.Port;
                var senderEmail = _emailSettings.SenderEmail ?? string.Empty;
                var senderName = string.IsNullOrWhiteSpace(_emailSettings.SenderName) ? "Complejo Gol Ahora" : _emailSettings.SenderName;
                var password = _emailSettings.Password ?? string.Empty;
                var enableSsl = _emailSettings.EnableSsl;

                using var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = enableSsl,
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

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string htmlBody, byte[] attachmentBytes, string attachmentName)
        {
            try
            {
                var smtpServer = string.IsNullOrWhiteSpace(_emailSettings.SmtpServer) ? "smtp.gmail.com" : _emailSettings.SmtpServer;
                var port = _emailSettings.Port <= 0 ? 587 : _emailSettings.Port;
                var senderEmail = _emailSettings.SenderEmail ?? string.Empty;
                var senderName = string.IsNullOrWhiteSpace(_emailSettings.SenderName) ? "Complejo Gol Ahora" : _emailSettings.SenderName;
                var password = _emailSettings.Password ?? string.Empty;
                var enableSsl = _emailSettings.EnableSsl;

                using var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = enableSsl,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                if (attachmentBytes != null && attachmentBytes.Length > 0)
                {
                    var stream = new MemoryStream(attachmentBytes);
                    var attachment = new Attachment(stream, attachmentName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);
                }

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
