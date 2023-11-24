using HotelBooking.Application.SharedInterfaces;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace HotelBooking.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string subject, string body, string To)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("chait8126po@gmail.email"));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text=body };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            string Password = Environment.GetEnvironmentVariable("POEMAILPASSWORD").ToString();
            smtp.Authenticate("chait8126po@gmail.com", Password);
            var response = await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
