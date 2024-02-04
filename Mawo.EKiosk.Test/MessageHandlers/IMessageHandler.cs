namespace Mawo.EKiosk.Test.MessageHandlers;

public interface IMessageHandler
{
	Message? HandleMessage(Message message);
}
