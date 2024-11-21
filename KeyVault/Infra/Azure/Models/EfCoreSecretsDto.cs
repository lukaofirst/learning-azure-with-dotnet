namespace Infra.Azure.Models;

public record EfCoreSecretsDto(
	string Host,
	string Port,
	string Database,
	string Username,
	string Password);
