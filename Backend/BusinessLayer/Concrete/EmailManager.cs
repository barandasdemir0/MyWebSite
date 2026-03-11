using BusinessLayer.Abstract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BusinessLayer.Concrete;

public class EmailManager : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Baran Dasdemir", _configuration["Email:From"]!));
        message.To.Add(MailboxAddress.Parse(to));

        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = body
        };

       

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _configuration["Email:Host"],
            int.Parse(_configuration["Email:Port"]!),
            SecureSocketOptions.StartTls,
            cancellationToken
            );

        await smtp.AuthenticateAsync(
            _configuration["Email:Username"],
            _configuration["Email:Password"],
            cancellationToken
            );

        await smtp.SendAsync(message, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);

       
    }
}
