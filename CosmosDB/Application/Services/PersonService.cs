using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infra.Repositories.Interfaces;

namespace Application.Services;

public class PersonService(IPersonRepository personRepository, IMapper mapper) : IPersonService
{
	private readonly IPersonRepository _personRepository = personRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<List<PersonViewModelDto>> GetAll()
	{
		var entities = await _personRepository.GetAll();
		var mappedEntities = _mapper.Map<List<PersonViewModelDto>>(entities);

		return mappedEntities;
	}

	public async Task<PersonViewModelDto?> GetById(string id)
	{
		var entity = await _personRepository.GetById(id);

		if (entity is null) return null!;

		var mappedEntity = _mapper.Map<PersonViewModelDto>(entity);

		return mappedEntity;
	}

	public async Task<Person> Create(PersonInputModelDto person)
	{
		var mappedEntity = _mapper.Map<Person>(person);
		var createdEntity = await _personRepository.Create(mappedEntity);

		return createdEntity;
	}

	public async Task<Person> Update(PersonViewModelDto person)
	{
		var mappedEntity = _mapper.Map<Person>(person);
		var updatedEntity = await _personRepository.Update(mappedEntity);

		return updatedEntity;
	}

	public async Task<bool> Delete(string id)
	{
		var isDeletedEntity = await _personRepository.Delete(id);

		return isDeletedEntity;
	}
}
