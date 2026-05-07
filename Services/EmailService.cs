using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace xAzubiLog.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendePasswortResetMail(string empfaenger, string resetLink)
        {
            Console.WriteLine("🔥 EMAIL SERVICE WIRD AUFGERUFEN");
            try
            {
                Console.WriteLine("SMTP START");

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("noreply@azubilog.de"));
                message.To.Add(MailboxAddress.Parse(empfaenger));
                message.Subject = "Passwort Reset";

                Console.WriteLine("Mail gebaut");

                using var client = new MailKit.Net.Smtp.SmtpClient();

                Console.WriteLine("Verbinde SMTP...");

                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                Console.WriteLine("Verbunden");

                await client.AuthenticateAsync("tamarazxbel@gmail.com", "sikp lnna eqdc nods");

                Console.WriteLine("Authentifiziert");

                await client.SendAsync(message);

                Console.WriteLine("Gesendet");

                await client.DisconnectAsync(true);

                Console.WriteLine("SMTP DONE");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SMTP ERROR:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
