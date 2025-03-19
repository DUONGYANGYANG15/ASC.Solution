using ASC.Solution.Configuration;
using Microsoft.Extensions.Options;
using MimeKit; // Đảm bảo chỉ dùng MimeKit
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using ASC.Web.Services;

namespace ASC.Solution.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeKit.MimeMessage(); // Sử dụng namespace rõ ràng
            emailMessage.From.Add(new MimeKit.MailboxAddress("Admin", _settings.Value.SMTPAccount));
            emailMessage.To.Add(new MimeKit.MailboxAddress(email, email)); // Đảm bảo truyền đủ tham số

            emailMessage.Subject = subject;
            emailMessage.Body = new MimeKit.TextPart("plain") { Text = message };

            using (var client = new MailKit.Net.Smtp.SmtpClient()) // Chỉ rõ MailKit
            {
                await client.ConnectAsync(_settings.Value.SMTPServer, _settings.Value.SMTPPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.CompletedTask;
        }
    }
}
