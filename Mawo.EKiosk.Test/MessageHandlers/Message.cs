using System.Text.Json.Serialization;

namespace Mawo.EKiosk.Test.MessageHandlers;

public class Message
{
	[JsonConstructor]
	public Message()
	{
	}

	public string? Event { get; set; }

	public string? Payload { get; set; }
}
