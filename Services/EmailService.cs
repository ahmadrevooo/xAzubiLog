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
            var message = new MimeMessage();

            // Gmail als echter Absender
            message.From.Add(
                new MailboxAddress(
                    "xAzubiLog",
                    "tamarazxbel@gmail.com"));

            message.To.Add(MailboxAddress.Parse(empfaenger));

            message.Subject = "Passwort zurücksetzen";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial; padding: 20px;'>

                    <h2>Passwort zurücksetzen</h2>

                    <p>
                        Du hast angefordert, dein Passwort zurückzusetzen.
                    </p>

                    <p>
                        Klicke auf den Button:
                    </p>

                    <a href='{resetLink}'
                       style='
                            display:inline-block;
                            padding:12px 20px;
                            background:#4CAF50;
                            color:white;
                            text-decoration:none;
                            border-radius:8px;
                            font-weight:bold;'>
                        Passwort zurücksetzen
                    </a>

                    <p style='margin-top:20px; color:gray;'>
                        Der Link ist 1 Stunde gültig.
                    </p>

                </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                "tamarazxbel@gmail.com",
                "sikp lnna eqdc nods");

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}