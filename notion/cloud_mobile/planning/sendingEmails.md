




Define an IEmailSender class, and register it in program.cs: 

`builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();
`

## EmailSender Class
``` using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using TetrisWeb.AuthData;

namespace TetrisWeb.ApiServices;
public class EmailSender : IEmailSender<ApplicationUser>
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress(user.UserName, email));
        emailMessage.Subject = "Confirm your email";

        emailMessage.Body = new TextPart("html")
        {
            Text = $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress(user.UserName, email));
        emailMessage.Subject = "Reset your password";

        emailMessage.Body = new TextPart("html")
        {
            Text = $"Your password reset code is: <strong>{resetCode}</strong>. Please use this code to reset your password."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);

    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress(user.UserName, email));
        emailMessage.Subject = "Reset your password";

        emailMessage.Body = new TextPart("html")
        {
            Text = $"Please reset your password by clicking <a href='{resetLink}'>here</a>."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}

```

I think (?) we installed the following packages: 
- MailKit.Net.Smtp;
- using MimeKit;


The following values should be set in appsettings.json:
```
 "EmailSettings": {
   "SmtpServer": "smtp.gmail.com",
   "Port": 587,
   "SenderName": "TetrisWeb Support",
   "SenderEmail": "<YourEmail>",
   "Username": "<YourEmail>",
   "Password": "<YourPassword>"
 }
```


The following configuration values should be set in user secrets:
```
"EmailSettings:SenderEmail": "tetris.arz@gmail.com",
"EmailSettings:Username": "tetris.arz@gmail.com",
"EmailSettings:Password": "[your app key generated by gmail]"
```

Getting the app key from Google (if you're using a gmail account):
https://security.google.com/settings/security/apppasswords

(NOTE: Two-Step Verification must be set up for your account in order to generate an app password.)