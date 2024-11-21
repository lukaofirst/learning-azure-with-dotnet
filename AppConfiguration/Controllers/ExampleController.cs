using Microsoft.AspNetCore.Mvc;

namespace AppConfiguration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExampleController(IConfiguration configuration) : ControllerBase
{
	[HttpGet]
	public IActionResult GetConfigValue()
	{
		var myConfigValue = configuration["ConfigKeyExample"];
		return Ok(myConfigValue);
	}
}
