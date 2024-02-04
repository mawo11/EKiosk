using Mawo.EKiosk.Test.MessageHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhotinoNET;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Mawo.EKiosk.Test;

[RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(JsonSerializerOptions)")]
[RequiresDynamicCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(JsonSerializerOptions)")]
internal class MainWindow : PhotinoWindow
{
	private readonly ILogger _logger;
	private readonly IMessageManager _messageManager;

	[RequiresUnreferencedCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.GetValue<T>(String, T)")]
	public MainWindow(IConfiguration configuration,
		ILogger logger,
		IMessageManager messageManager) : base(null)
	{
		_logger = logger;
		_messageManager = messageManager;

		FullScreen = true;
		Chromeless = true;
		Resizable = false;
		Title = "EKiosk";
		ContextMenuEnabled = configuration.GetValue("runtime:dev", false);

		RegisterWebMessageReceivedHandler(OnMessagesReceived);

		_logger.LogInformation("Starting EKiosk...");
	}

	private void OnMessagesReceived(object? sender, string serializedMessage)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("Received message: \"{message}\"", serializedMessage);
		}

		if (string.IsNullOrEmpty(serializedMessage))
		{
			return;
		}

		try
		{
			Message? sourceMessage = JsonSerializer.Deserialize<Message>(serializedMessage, SourceGenerationContext.Default.Message);
			if (sourceMessage is null)
			{
				_logger.LogWarning("Failed to deserialize message: \"{message}\"", serializedMessage);
				return;
			}

			Message? responseMessage = _messageManager.HandleMessage(sourceMessage);

			if (responseMessage is not null)
			{
				string serializedResponse = JsonSerializer.Serialize(responseMessage, SourceGenerationContext.Default.Message);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("Response message: \"{response}\"", serializedResponse);
				}

				PhotinoWindow? window = (PhotinoWindow?)sender;
				window?.SendWebMessage(serializedResponse);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while processing message: \"{message}\"", serializedMessage);
		}
	}
}
