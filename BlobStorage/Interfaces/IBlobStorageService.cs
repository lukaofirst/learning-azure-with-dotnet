namespace BlobStorage.Interfaces;

public interface IBlobStorageService
{
	Task<(byte[] fileBytes, string contentType)> GetFileAsync(string fileName);
	Task<bool> UploadFileAsync(IFormFile formFile);
	Task<bool> DeleteFileAsync(string fileName);
}
