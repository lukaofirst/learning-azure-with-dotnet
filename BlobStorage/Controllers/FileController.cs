using BlobStorage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController(IBlobStorageService blobStorageService) : ControllerBase
{
	[HttpGet("{fileName}")]
	public async Task<IActionResult> Get(string fileName)
	{
		try
		{
			var (fileBytes, contentType) = await blobStorageService.GetFileAsync(fileName);

			return File(fileBytes, contentType);
		}
		catch (Exception ex)
		{
			return BadRequest(ex);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Post(IFormFile formFile)
	{
		try
		{
			var result = await blobStorageService.UploadFileAsync(formFile);

			return Ok(result);
		}
		catch (Exception ex)
		{
			return BadRequest(ex);
		}
	}

	[HttpDelete("{fileName}")]
	public async Task<IActionResult> Delete(string fileName)
	{
		try
		{
			var result = await blobStorageService.DeleteFileAsync(fileName);

			return Ok(result);
		}
		catch (Exception ex)
		{
			return BadRequest(ex);
		}
	}
}
