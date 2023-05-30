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
        UploadFile();
    }

    public async void UploadFile() {
        // Fetch the Connection String from your configuration
        string connectionString = _configuration.GetConnectionString("AzureBlob");

        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        // Create the container and return a container client object
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("firstblobcontainer");

        // Create a local file in the ./data/ directory for uploading and downloading
        string localPath = "./text_files/";
        string fileName = "first_uploaded.txt";
        string localFilePath = Path.Combine(localPath, fileName);

        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        // Open the file and upload its data
        await blobClient.UploadAsync(localFilePath, true);
    }
}

