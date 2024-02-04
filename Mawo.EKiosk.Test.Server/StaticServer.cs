using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Mawo.EKiosk.Test.Server;

public static class StaticServer
{
	public static WebApplication CreateStaticFileServer(string[] args, int startPort, int portRange, out string baseUrl)
	{
		const string webRootFolder = "wwwroot";
		WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(new WebApplicationOptions
		{
			Args = args,
			WebRootPath = webRootFolder
		});

		ManifestEmbeddedFileProvider manifestEmbeddedFileProvider = new ManifestEmbeddedFileProvider(Assembly.GetEntryAssembly()!, "Resources/" + webRootFolder);
		IFileProvider webRootFileProvider = webApplicationBuilder.Environment.WebRootFileProvider;
		CompositeFileProvider webRootFileProvider2 = new CompositeFileProvider(manifestEmbeddedFileProvider, webRootFileProvider);
		webApplicationBuilder.Environment.WebRootFileProvider = webRootFileProvider2;
		int port;
		for (port = startPort; IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any((IPEndPoint x) => x.Port == port); port++)
		{
			if (port > port + portRange)
			{
				throw new SystemException($"Couldn't find open port within range {port - portRange} - {port}.");
			}
		}

		baseUrl = $"http://localhost:{port}";
		webApplicationBuilder.WebHost.UseUrls(baseUrl);
		WebApplication webApplication = webApplicationBuilder.Build();
		webApplication.UseDefaultFiles();
		webApplication.UseStaticFiles(new StaticFileOptions
		{
			DefaultContentType = "text/plain"
		});
		return webApplication;
	}
}
