using EmailAuthenticator;
using MimeKit;

namespace Consilium.API.DBServices;

public class EmailService(IConfiguration config) : IEmailService {
    public async Task SendValidationEmail(string email, string validationToken) {
        string link = $"http://localhost:5202/account/validate?email={email}&token={validationToken}";
        Console.WriteLine(link);

        await SendEmail(email, link);
    }

    private async Task SendEmail(string email, string link) {

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(config["EmailSettings:SenderName"], config["EmailSettings:SenderEmail"]));

        string username = email.Split('@')[0];
        emailMessage.To.Add(new MailboxAddress(username, email));
        emailMessage.Subject = "Confirm your email";

        emailMessage.Body = new TextPart("html") {
            Text = $"Please confirm your account by clicking <a href='{link}'>here</a>."
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        if (config["EmailSettings:Port"] is null) {
            throw new Exception("Port cannot be null");
        }
        int port = int.Parse(config["EmailSettings:Port"]!);
        await client.ConnectAsync(config["EmailSettings:SmtpServer"], port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(config["EmailSettings:Username"], config["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}