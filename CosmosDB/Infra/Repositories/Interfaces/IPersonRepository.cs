using Domain.Entities;

namespace Infra.Repositories.Interfaces;

public interface IPersonRepository
{
	Task<List<Person>> GetAll();
	Task<Person?> GetById(string id);
	Task<Person> Create(Person person);
	Task<Person> Update(Person person);
	Task<bool> Delete(string id);
}
