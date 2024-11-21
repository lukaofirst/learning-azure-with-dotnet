using Domain.Entities;

namespace Infra.Repositories.Interfaces;

public interface IPersonRepository
{
	Task<List<Person>> GetAll();
	Task<Person>? GetById(int id);
	Task<Person> Create(Person person);
	Task<Person> Update(Person person);
	Task<bool> Delete(int id);
}
