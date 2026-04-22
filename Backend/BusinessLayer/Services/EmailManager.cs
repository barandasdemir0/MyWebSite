using BusinessLayer.Abstract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BusinessLayer.Services;

public class EmailManager : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    //türkçe : // Bu metot, belirtilen alıcıya, konuya ve içeriğe sahip bir e-posta göndermek için kullanılır.
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // E-posta mesajını oluştur
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Baran Dasdemir", _configuration["Email:From"]!)); // Gönderen adresini yapılandırmadan al
        message.To.Add(MailboxAddress.Parse(to));// Alıcı adresini ekle

        message.Subject = subject; // E-posta konusunu belirle
        message.Body = new TextPart("html") // E-posta içeriğini HTML formatında belirle
        {
            Text = body // E-posta içeriği
        };

       

        using var smtp = new SmtpClient(); // SMTP istemcisi oluştur
        await smtp.ConnectAsync( // SMTP sunucusuna bağlan
            _configuration["Email:Host"]!, // SMTP sunucusunun adresini yapılandırmadan al
            int.Parse(_configuration["Email:Port"]!), // SMTP sunucusunun portunu yapılandırmadan al
            SecureSocketOptions.StartTls,// Güvenli bağlantı seçeneklerini belirle
            cancellationToken // Bağlantı sırasında iptal işlemi için kullanılan token
            );

        await smtp.AuthenticateAsync(
            _configuration["Email:Username"]!,// SMTP sunucusu için kullanıcı adını yapılandırmadan al
            _configuration["Email:Password"]!, // SMTP sunucusu için şifreyi yapılandırmadan al
            cancellationToken
            );

        await smtp.SendAsync(message, cancellationToken);// E-postayı gönder
        await smtp.DisconnectAsync(true, cancellationToken); // SMTP sunucusundan güvenli bir şekilde bağlantıyı kes


    }
}
