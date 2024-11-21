using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPersonService
{
	Task<List<PersonViewModelDto>> GetAll();
	Task<PersonViewModelDto?> GetById(string id);
	Task<Person> Create(PersonInputModelDto person);
	Task<Person> Update(PersonViewModelDto person);
	Task<bool> Delete(string id);
}
