using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Messaging.ServiceBus;

namespace AzureShowcase.Pages;

public class QueueUploadModel : PageModel
{
    private readonly IConfiguration _configuration;
    
    public QueueUploadModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync(IFormFile uploadedFile) {
        await SendFileToQueue(uploadedFile);
    }

    public async Task SendFileToQueue(IFormFile uploadedFile) {

        string connectionString = _configuration.GetConnectionString("AzureServiceBus");

        // create client and sender
        ServiceBusClient client = new ServiceBusClient(connectionString);
        ServiceBusSender sender = client.CreateSender("firstqueue");

        using var reader = new StreamReader(uploadedFile.OpenReadStream());
        var textMessage = await reader.ReadToEndAsync();

        // create the message
        ServiceBusMessage message = new ServiceBusMessage(textMessage);

         // send the message.  Note that this will queue the message for later processing
        await sender.SendMessageAsync(message);
    }
}

