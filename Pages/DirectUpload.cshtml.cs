using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;

namespace AzureShowcase.Pages;

public class DirectUploadModel : PageModel
{
    private readonly IConfiguration _configuration;
    
    public DirectUploadModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync(IFormFile uploadedFile) {
        await UploadFile(uploadedFile);
    }

    public async Task UploadFile(IFormFile uploadedFile) {

        // Fetch the Connection String from your configuration
        string connectionString = _configuration.GetConnectionString("AzureBlob");

        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        // Create the container and return a container client object
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("firstblobcontainer");

        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(uploadedFile.FileName);

        using var stream = uploadedFile.OpenReadStream(); // File to read from.
        // Open the file and upload its data
        await blobClient.UploadAsync(stream, true);
    }
}

