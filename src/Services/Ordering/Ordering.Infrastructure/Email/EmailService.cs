namespace Ordering.Infrastructure.Email
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ordering.Application.Contracts.Infrastructure;
    using Ordering.Application.Models;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings.Value;
            this.logger = logger;
        }

        public async Task<bool> SendEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            var client = new SendGridClient(emailSettings.ApiKey);
            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var body = email.Body;
            var from = new EmailAddress
            {
                Email = emailSettings.FromAddress,
                Name = emailSettings.FromName
            };
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(sendGridMessage, cancellationToken);

            logger.LogInformation("Email sent.");

            if (response.StatusCode == HttpStatusCode.Accepted
                || response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            logger.LogError("Email sending failed.");

            return false;
        }
    }
}
