namespace Infra.Azure.Interfaces;

public interface IAzureKeyVaultService
{
	Task<string> GetSecretAsync(string secretName);
}
