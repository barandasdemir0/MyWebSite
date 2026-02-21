using BusinessLayer.Abstract;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

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
        using var smtp = new SmtpClient(_configuration["Email:Host"])
        {
            Port = int.Parse(_configuration["Email:Host"]!),
            Credentials = new NetworkCredential
            (
                _configuration["Email:Username"],
                _configuration["Email::Password"]),
            EnableSsl = true
        };
        var mail = new MailMessage(_configuration["Email:From"]!, to, subject, body);
        await smtp.SendMailAsync(mail, cancellationToken);
    }
}
