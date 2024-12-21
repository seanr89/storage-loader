

using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Azure.Storage;
using Azure.Storage.Blobs;

public class AzureBlobService
{
    private readonly string _storageAccount = "";
    private readonly string _storageKey = "";
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobService(){

        var cred = new StorageSharedKeyCredential(_storageAccount, _storageKey);
        var blobUri = new Uri($"https://{_storageAccount}.blob.core.windows.net");
        _blobServiceClient = new BlobServiceClient(blobUri, cred);

    }

    public async Task QueryAndListBlobsAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        await containerClient.CreateIfNotExistsAsync();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine($"Blob: {blobItem.Name}");
        }
    }

    public async Task UploadFileAsync(string filePath)
    {
        var blobUrsIs = new List<Uri>();
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(filePath.Split('/').Last());

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

        // Upload data from the local file, overwrite the blob if it already exists
        await blobClient.UploadAsync(filePath, true);
    }
}