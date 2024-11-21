using System.Text.Json.Serialization;

namespace Domain.Entities;

public class DomainMessage
{
	[JsonInclude]
	public Guid Id { get; private set; } = Guid.NewGuid();
	public string? Body { get; set; }
	[JsonInclude]
	public DateTime CurrentTimeStamp { get; private set; } = DateTime.Now;

	public override string ToString()
	{
		return $"Id - {Id} | Body - {Body} | CurrentTimeStamp - {CurrentTimeStamp}";
	}
}
