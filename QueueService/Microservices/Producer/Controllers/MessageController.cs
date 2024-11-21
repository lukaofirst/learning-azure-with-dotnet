using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Producer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(
	IPublishService publishService,
	IOptions<AzureServices> options) : ControllerBase
{
	[HttpGet]
	public IActionResult Check()
	{
		var azureServices = options.Value;

		return Ok(azureServices);
	}

	[HttpPost]
	public async Task<IActionResult> PostMessage(DomainMessage message)
	{
		try
		{
			var result = await publishService.SendAsync(message);

			return Ok(result);
		}
		catch (Exception ex)
		{
			return BadRequest(ex);
		}
	}
}
