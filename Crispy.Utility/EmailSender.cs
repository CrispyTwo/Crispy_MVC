using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Crispy.Utility
{
    public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage(_emailSettings.FromEmail, email, subject, htmlMessage);

                return client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log exception or debug
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
