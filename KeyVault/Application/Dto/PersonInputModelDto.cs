namespace Application.Dto;

public record PersonInputModelDto
{
	public string? Name { get; init; }
	public int Age { get; init; }
	public string? Email { get; init; }
}
