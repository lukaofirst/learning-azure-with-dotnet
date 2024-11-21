namespace Domain.Entities;

public class Person
{
	public int Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public int Age { get; private set; }
	public string? Email { get; private set; }

	public void UpdateFields(Person updatedPerson)
	{
		Name = updatedPerson.Name;
		Age = updatedPerson.Age;
		Email = updatedPerson.Email;
	}
}
