using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService) : ControllerBase
{
	private readonly IPersonService _personService = personService;

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var entities = await _personService.GetAll();

		return Ok(entities);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		try
		{
			var entity = await _personService.GetById(id);

			return Ok(entity);
		}
		catch (Exception ex)
		{
			return BadRequest(ex);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Post(PersonInputModelDto person)
	{
		var createdEntity = await _personService.Create(person);

		return Ok(createdEntity);
	}

	[HttpPut]
	public async Task<IActionResult> Update(PersonViewModelDto person)
	{
		var updatedEntity = await _personService.Update(person);

		return Ok(updatedEntity);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(int id)
	{
		var isDeletedEntity = await _personService.Delete(id);

		return Ok(isDeletedEntity);
	}
}
