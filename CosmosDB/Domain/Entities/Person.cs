namespace Domain.Entities;

public class Person
{
	public string? id { get; set; }
	public string? Name { get; set; }
	public int Age { get; set; }
	public string? Email { get; set; }

	public Person() { }

	public void SetId(string gid)
	{
		id = gid;
	}
}
