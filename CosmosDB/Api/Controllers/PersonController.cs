using System.ComponentModel.DataAnnotations;
using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService) : ControllerBase
{
	[HttpGet(Name = "GetAll")]
	public async Task<IActionResult> GetAll()
	{
		var entities = await personService.GetAll();

		return Ok(entities);
	}

	[HttpGet("{id}", Name = "GetById")]
	public async Task<IActionResult> GetById([Required] string id)
	{
		var entity = await personService.GetById(id);

		return entity is not null ? Ok(entity) : NotFound();
	}

	[HttpPost]
	public async Task<IActionResult> Post(PersonInputModelDto person)
	{
		var createdEntity = await personService.Create(person);

		return Ok(createdEntity);
	}

	[HttpPut]
	public async Task<IActionResult> Update(PersonViewModelDto person)
	{
		var updatedEntity = await personService.Update(person);

		return Ok(updatedEntity);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete([Required] string id)
	{
		var isDeletedEntity = await personService.Delete(id);

		return Ok(isDeletedEntity);
	}
}
