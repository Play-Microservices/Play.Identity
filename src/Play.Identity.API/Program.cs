using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Play.Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                if (hostingContext.HostingEnvironment.IsProduction())
                {
                    configurationBuilder.AddAzureKeyVault(
                        new Uri("https://playmicroservices.vault.azure.net/"),
                        new DefaultAzureCredential()
                    );
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
