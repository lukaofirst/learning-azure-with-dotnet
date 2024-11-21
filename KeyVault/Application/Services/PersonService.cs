using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infra.Repositories.Interfaces;

namespace Application.Services;

public class PersonService(IPersonRepository personRepository, IMapper mapper) : IPersonService
{
	public async Task<List<PersonViewModelDto>> GetAll()
	{
		var entities = await personRepository.GetAll();
		var mappedEntities = mapper.Map<List<PersonViewModelDto>>(entities);

		return mappedEntities;
	}

	public async Task<PersonViewModelDto> GetById(int id)
	{
		var entity = await personRepository.GetById(id)!
			?? throw new Exception($"{nameof(Person)} entity is null");

		var mappedEntity = mapper.Map<PersonViewModelDto>(entity);

		return mappedEntity;
	}

	public async Task<Person> Create(PersonInputModelDto person)
	{
		var mappedEntity = mapper.Map<Person>(person);
		var createdEntity = await personRepository.Create(mappedEntity);

		return createdEntity;
	}

	public async Task<Person> Update(PersonViewModelDto person)
	{
		var mappedEntity = mapper.Map<Person>(person);
		var updatedEntity = await personRepository.Update(mappedEntity);

		return updatedEntity;
	}

	public async Task<bool> Delete(int id)
	{
		var isDeletedEntity = await personRepository.Delete(id);

		return isDeletedEntity;
	}
}
