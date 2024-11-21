using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage.Interfaces;

namespace BlobStorage.Services;

public class BlobStorageService(
	BlobServiceClient blobServiceClient, IConfiguration configuration)
	: IBlobStorageService
{
	private readonly string _blobContainerName = configuration["AzureBlob:ContainerName"]!;

	// Get file from Azure Blob Storage
	public async Task<(byte[] fileBytes, string contentType)> GetFileAsync(string fileName)
	{
		var containerClient = blobServiceClient.GetBlobContainerClient(_blobContainerName);
		var blobClient = containerClient.GetBlobClient(fileName);

		var response = await blobClient.DownloadAsync();

		if (!response.GetRawResponse().Status.Equals((int)HttpStatusCode.OK))
			throw new Exception("Could not retrieve file from Azure Blob Storage");

		using var ms = new MemoryStream();
		await response.Value.Content.CopyToAsync(ms);

		return (ms.ToArray(), response.Value.ContentType);
	}

	public async Task<bool> UploadFileAsync(IFormFile formFile)
	{
		using var ms = new MemoryStream();
		await formFile.CopyToAsync(ms);

		var containerClient = blobServiceClient.GetBlobContainerClient(_blobContainerName);
		var blobClient = containerClient.GetBlobClient(formFile.FileName);

		ms.Position = 0;
		var response = await blobClient.UploadAsync(ms, new BlobHttpHeaders { ContentType = formFile.ContentType });

		return response.GetRawResponse().Status.Equals((int)HttpStatusCode.Created);
	}

	public async Task<bool> DeleteFileAsync(string fileName)
	{
		var containerClient = blobServiceClient.GetBlobContainerClient(_blobContainerName);
		var blobClient = containerClient.GetBlobClient(fileName);

		var response = await blobClient.DeleteIfExistsAsync();

		return response.Value;
	}
}
