using System.Net.Mail;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace SendEmailBlobTrigger
{
    public class SendEmailBlobTriggerFunction
    {
        private readonly ILogger<SendEmailBlobTriggerFunction> _logger;

        public SendEmailBlobTriggerFunction(ILogger<SendEmailBlobTriggerFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SendEmailBlobTriggerFunction))]
        public async Task Run([BlobTrigger("firstblobcontainer/{name}", Connection = "BlobStorageConnection")] Stream stream, string name)
        {
            _logger.LogInformation($"Blob trigger function processed blob\n Name:{name} \n ");

            // Read the blob content
            StreamReader reader = new StreamReader(stream);
            string content = await reader.ReadToEndAsync();

            // Create api client
            var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            var client = new SendGridClient(apiKey);

            // Configure the email
            var fromEmail = Environment.GetEnvironmentVariable("SenderEmail");
            var fromName = Environment.GetEnvironmentVariable("SenderName");
            var from = new EmailAddress(fromEmail, fromName);

            var toEmail = Environment.GetEnvironmentVariable("ReceiverEmail");
            var toName = Environment.GetEnvironmentVariable("ReceiverName");
            var to = new EmailAddress(toEmail, toName);

            var subject = Environment.GetEnvironmentVariable("EmailSubject");

            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            try
            {
                // Send the email
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                _logger.LogError($"Email sending failed: {ex.Message}");
            }
        }
    }
}
