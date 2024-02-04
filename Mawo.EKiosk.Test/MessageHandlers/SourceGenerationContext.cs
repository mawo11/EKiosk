using System.Text.Json.Serialization;

namespace Mawo.EKiosk.Test.MessageHandlers;

[JsonSourceGenerationOptions(WriteIndented = true,
	PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
	PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(Message))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
