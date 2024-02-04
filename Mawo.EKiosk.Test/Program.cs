using Mawo.EKiosk.Test.MessageHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Mawo.EKiosk.Test;

internal static class Program
{
	[STAThread]
	[RequiresUnreferencedCode("Calls Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<TService>()")]
	private static void Main(string[] args)
	{
#if DEBUG
		string baseUrl = "http://localhost:5173";
#else
		Mawo.EKiosk.Test.Server.StaticServer
		   .CreateStaticFileServer(args, 8000, 100, out string baseUrl)
			.RunAsync();
#endif
		var configurationBuilder = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

		var configuration = configurationBuilder.Build();
		using var loggerFactory = LoggerFactory.Create(cfg =>
		{
			cfg.AddConfiguration(configuration.GetSection("Logging"));
			cfg.AddConsole();
			cfg.AddNLog();
		});

		var services = new ServiceCollection();
		services.AddSingleton<MainWindow>();
		services.AddSingleton<IConfiguration>(configuration);
		services.AddSingleton(loggerFactory.CreateLogger("EKiosk"));
		services.AddSingleton<IMessageManager, MessageManager>();
		services.AddTransient<TestMessage>();


		using ServiceProvider serviceProvider = services.BuildServiceProvider();

		var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Load(baseUrl);
		mainWindow.WaitForClose();
	}
}