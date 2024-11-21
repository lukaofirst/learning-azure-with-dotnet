namespace Application.Dto;

public record PersonViewModelDto
{
	public int Id { get; init; }
	public string? Name { get; init; }
	public int Age { get; init; }
	public string? Email { get; init; }
}
