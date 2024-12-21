
using Azure.Storage;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    public async Task UploadListToBlobAsync()
    {
        List<Trans> fileData = new List<Trans>();
        fileData.Add(new Trans { Name = "Hello" });
        fileData.Add(new Trans { Name = "World" });
        var stringData = JsonConvert.SerializeObject(fileData);
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        BlobClient blobClient = containerClient.GetBlobClient("testfile.json");

        _ = await blobClient.UploadAsync(BinaryData.FromString(stringData), overwrite: true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    public async Task UploadListToBlobAsync(List<string> fileData)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        BlobClient blobClient = containerClient.GetBlobClient("testfile.txt");

        _ = await blobClient.UploadAsync(BinaryData.FromString(fileData.ToString()), overwrite: true);
    }

    public async Task DownloadFileAsync(string filePath)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("dailyupdates");
        BlobClient blobClient = containerClient.GetBlobClient("transactions.csv");

        Console.WriteLine("Downloading blob to\n\t{0}\n", filePath);

        // Download the blob's contents and save it to a file
        await blobClient.DownloadToAsync(filePath);
    }

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