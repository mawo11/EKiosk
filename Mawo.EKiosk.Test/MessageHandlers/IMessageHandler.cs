namespace Mawo.EKiosk.Test.MessageHandlers;

internal interface IMessageHandler
{
	object? HandleMessage(Message message);
}
