using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
            Console.WriteLine($"SMTP HOST__ {_config["Smtp__Host"]}");
            Console.WriteLine($"SMTP USER__ {_config["Smtp__User"]}");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("xAzubiLog", _config["Smtp__User"]));
            message.To.Add(MailboxAddress.Parse(empfaenger));
            message.Subject = "Passwort zurücksetzen";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial; padding: 20px;'>
                    <h2>Passwort zurücksetzen</h2>
                    <p>Du hast angefordert, dein Passwort zurückzusetzen.</p>
                    <p>Klicke auf den Button:</p>
                    <a href='{resetLink}'
                       style='display:inline-block;padding:12px 20px;
                              background:#4CAF50;color:white;
                              text-decoration:none;border-radius:8px;
                              font-weight:bold;'>
                        Passwort zurücksetzen
                    </a>
                    <p style='margin-top:20px;color:gray;'>
                        Der Link ist 1 Stunde gültig.
                    </p>
                </div>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["Smtp__Host"],
                int.Parse(_config["Smtp__Port"]!),
                SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                _config["Smtp__User"],
                _config["Smtp__Pass"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}