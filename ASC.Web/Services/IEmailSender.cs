using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Đây chỉ là demo, bạn có thể tích hợp với SMTP, SendGrid, hoặc API Email
        Console.WriteLine($"Gửi email tới: {email}, Chủ đề: {subject}");
        return Task.CompletedTask; // Trả về một Task hoàn thành ngay lập tức
    }
}
