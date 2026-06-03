using System;
using System.Net;
using System.Net.Mail;

namespace TestSmtp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Probando conexión SMTP con complejogolahora@gmail.com...");
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("complejogolahora@gmail.com", "tgcf xugs czdh ekpx"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("complejogolahora@gmail.com", "Prueba Gol Ahora"),
                    Subject = "Prueba de Depuración SMTP",
                    Body = "Esta es una prueba de envío SMTP desde la consola de depuración.",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add("complejogolahora@gmail.com");

                smtpClient.Send(mailMessage);
                Console.WriteLine("¡ÉXITO! El correo de prueba fue enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n¡ERROR DETECTADO EN SMTP!");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
