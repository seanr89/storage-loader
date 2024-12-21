// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var azureBlobService = new AzureBlobService();

await azureBlobService.QueryAndListBlobsAsync();
// Simple file write!!
await azureBlobService.UploadFileAsync("../files/transactions.csv");

Console.WriteLine("Press any key to exit");
Console.ReadKey();