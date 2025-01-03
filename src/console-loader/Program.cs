﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var azureBlobService = new AzureBlobService();

await azureBlobService.QueryAndListBlobsAsync();

// Simple file write!!
// await azureBlobService.UploadFileAsync("../files/transactions.csv");

// await azureBlobService.DownloadFileAsync("../files/transactions-downloaded.csv");

await azureBlobService.UploadListToBlobAsync();

// Now we will search and delete the blobs
//await azureBlobService.SearchAndDeleteBlobAsync();

Console.WriteLine("Press any key to exit");
Console.ReadKey();