using System.Net;
using Domain.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Infra.Repositories;

public class PersonRepository : IPersonRepository
{
	private readonly Container _container;

	public PersonRepository(CosmosClient cosmosClient, IConfiguration configuration)
	{
		var databaseName = configuration["CosmosDb:DatabaseName"];
		var containerName = configuration["CosmosDb:ContainerName"];
		_container = cosmosClient.GetContainer(databaseName, containerName);
	}

	public async Task<List<Person>> GetAll()
	{
		var query = "SELECT * FROM c";
		var queryDefinition = new QueryDefinition(query);
		var queryResultSetIterator = _container.GetItemQueryIterator<Person>(queryDefinition);

		var results = new List<Person>();
		while (queryResultSetIterator.HasMoreResults)
		{
			var response = await queryResultSetIterator.ReadNextAsync();
			results.AddRange(response);
		}

		return results;
	}

	public async Task<Person?> GetById(string id)
	{
		try
		{
			var response = await _container.ReadItemAsync<Person>(id, new PartitionKey(id));
			return response.Resource;
		}
		catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return null;
		}
	}

	public async Task<Person> Create(Person person)
	{
		person.SetId(Guid.NewGuid().ToString());

		var response = await _container.CreateItemAsync(person, new PartitionKey(person.id));

		return response.Resource;
	}

	public async Task<Person> Update(Person person)
	{
		var response = await _container.UpsertItemAsync(person, new PartitionKey(person.id));

		return response.Resource;
	}

	public async Task<bool> Delete(string id)
	{
		var result = await _container.DeleteItemAsync<Person>(id, new PartitionKey(id));
		return result.StatusCode.Equals(HttpStatusCode.NoContent);
	}
}

