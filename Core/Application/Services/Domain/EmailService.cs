using Application.Interfaces.Services.Domain;
using Domain.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Application.Services.Domain
{
    public class EmailService : IEmailService
    {
        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? EnvironmentManager.GetSMTPEmail()));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(EnvironmentManager.GetSMTPHost(), Convert.ToInt32(EnvironmentManager.GetSMTPPort()), SecureSocketOptions.StartTls);
            smtp.Authenticate(EnvironmentManager.GetSMTPUser(), EnvironmentManager.GetSMTPPassword());
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}