namespace Mawo.EKiosk.Test.MessageHandlers
{
	public interface IMessageManager
	{
		Message? HandleMessage(Message message);
	}
}