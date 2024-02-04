namespace Mawo.EKiosk.Test.MessageHandlers;

internal sealed class TestMessage : IMessageHandler
{
	public Message? HandleMessage(Message message)
	{
		return new Message
		{
			Event = $"{message?.Event}-response",
			Payload = $"Response from app: {DateTime.Now.Ticks}"
		};
	}
}
