using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureFunctionsShowcase
{
    public class CopyQueueMessageToBlob
    {
        private readonly ILogger<CopyQueueMessageToBlob> _logger;

        public CopyQueueMessageToBlob(ILogger<CopyQueueMessageToBlob> logger)
        {
            _logger = logger;
        }

        [Function(nameof(CopyQueueMessageToBlob))]
        public void Run([ServiceBusTrigger("firstqueue", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Blob Storage connection string
            var storageConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnection");

            _logger.LogInformation("Connecting to Blob Storage...");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainerName = Environment.GetEnvironmentVariable("BlobContainerName");
            CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName);

            // Create a new block blob and give it a unique name
            CloudBlockBlob blob = container.GetBlockBlobReference($"{Guid.NewGuid()}.txt");

            // Upload the message content to the blob
            blob.UploadTextAsync(message.Body.ToString());
        }
    }
}
