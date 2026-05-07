using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace xAzubiLog.Services
{
    public class EmailService
    {
        public async Task SendePasswortResetMail(string empfaenger, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("noreply@azubilog.de"));
            message.To.Add(MailboxAddress.Parse(empfaenger));
            message.Subject = "Passwort zurücksetzen";

            message.Body = new TextPart("plain")
            {
                Text = $"Setze dein Passwort hier zurück:\n\n{resetLink}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            // HIER DEINE DATEN EINTRAGEN
            await client.AuthenticateAsync("tamarazxbel@gmail.com", "mhot fepx ljpo gagb");

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}