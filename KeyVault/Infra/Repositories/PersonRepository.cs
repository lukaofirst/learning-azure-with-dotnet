using Domain.Entities;
using Infra.EFCore;
using Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class PersonRepository(DataContext dataContext) : IPersonRepository
{
	public async Task<List<Person>> GetAll()
	{
		var entities = await dataContext.Persons
			.AsNoTracking()
			.ToListAsync();
		return entities;
	}

	public async Task<Person>? GetById(int id)
	{
		var entity = await dataContext.Persons
			.AsNoTracking()
			.Where(x => x.Id.Equals(id))
			.FirstOrDefaultAsync();

		return entity!;
	}

	public async Task<Person> Create(Person person)
	{
		await dataContext.Persons.AddAsync(person);

		var result = await dataContext.SaveChangesAsync();

		return result > 0 ? person : throw new Exception("Could not insert person!");
	}

	public async Task<Person> Update(Person updatedPerson)
	{
		var person = await dataContext.Persons
			.Where(x => x.Id.Equals(updatedPerson.Id))
			.FirstOrDefaultAsync()
				?? throw new Exception("Could not update person entity, because person is null");

		person.UpdateFields(updatedPerson);

		dataContext.Persons.Update(person!);

		var result = await dataContext.SaveChangesAsync();

		return result > 0 ? person : throw new Exception("Could not update person entity");
	}

	public async Task<bool> Delete(int id)
	{
		var person = await dataContext.Persons
			.Where(x => x.Id.Equals(id))
			.FirstOrDefaultAsync()
				?? throw new Exception("Could not delete person entity, because person is null");

		dataContext.Persons.Remove(person);

		var result = await dataContext.SaveChangesAsync();

		return result > 0;
	}
}
