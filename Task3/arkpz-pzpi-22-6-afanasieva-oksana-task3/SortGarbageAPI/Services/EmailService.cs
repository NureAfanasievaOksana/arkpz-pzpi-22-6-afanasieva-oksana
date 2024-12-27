using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service for sending email notifications
    /// </summary>
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the EmailService
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        /// <param name="toEmail">Recipient's email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        /// <returns>A Task representing the asynchronous operation</returns>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["Email:Host"];
            var smtpPort = int.Parse(_configuration["Email:Port"]);
            var fromEmail = _configuration["Email:FromEmail"];
            var password = _configuration["Email:Password"];

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(toEmail);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
            }
        }
    }
}