namespace Mawo.EKiosk.Test.MessageHandlers;

public sealed class MessageManager : IMessageManager
{
	private static readonly Dictionary<string, Type> _messageHandlers = new()
	{
		{ "test-message", typeof(TestMessage) }
	};

	private readonly IServiceProvider _serviceProvider;

	public MessageManager(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public Message? HandleMessage(Message message)
	{
		if (string.IsNullOrEmpty(message.Event))
		{
			return null;
		}

		if (_messageHandlers.TryGetValue(message.Event, out Type? handlerType))
		{
			IMessageHandler? handler = _serviceProvider.GetService(handlerType) as IMessageHandler;
			return handler?.HandleMessage(message);
		}

		return null;
	}
}
