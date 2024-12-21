

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

    /// <summary>
    /// Run Query and List Blobs
    /// </summary>
    /// <returns></returns>
    public async Task QueryAndListBlobsAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        await containerClient.CreateIfNotExistsAsync();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine($"Found Blob: {blobItem.Name}");
        }
    }

    /// <summary>
    /// Upload File to Blob Storage
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task UploadFileAsync(string filePath)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(filePath.Split('/').Last());

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

        // Upload data from the local file, overwrite the blob if it already exists
        await blobClient.UploadAsync(filePath, true);
    }

    // public async Task DownloadFileAsync(string filePath)
    // {
    //     var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
    //     BlobClient blobClient = containerClient.GetBlobClient(filePath.Split('/').Last());

    //     Console.WriteLine("Downloading blob to\n\t{0}\n", filePath);

    //     // Download the blob's contents and save it to a file
    //     await blobClient.DownloadToAsync(filePath);
    // }

    public async Task SearchAndDeleteBlobAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        await containerClient.CreateIfNotExistsAsync();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine($"Deleting Blob: {blobItem.Name}");
            await containerClient.DeleteBlobAsync(blobItem.Name);
        }
    }
}