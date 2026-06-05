using System;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string htmlBody, byte[] attachmentBytes, string attachmentName);
    }
}
