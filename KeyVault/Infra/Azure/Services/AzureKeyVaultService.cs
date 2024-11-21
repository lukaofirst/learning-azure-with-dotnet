using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infra.Azure.Interfaces;

namespace Infra.Azure.Services;

public class AzureKeyVaultService(string keyVaultUri) : IAzureKeyVaultService
{
	private readonly SecretClient _secretClient = new(new Uri(keyVaultUri), new DefaultAzureCredential());

	public async Task<string> GetSecretAsync(string secretName)
	{
		try
		{
			var secret = await _secretClient.GetSecretAsync(secretName);
			return secret.Value.Value;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error retrieving secret '{secretName}': {ex.Message}", ex);
		}
	}
}
