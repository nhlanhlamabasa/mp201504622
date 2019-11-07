using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Diagnostics.Process CurrentPro = new System.Diagnostics.Process();

            CurrentPro = System.Diagnostics.Process.GetCurrentProcess();

            Console.WriteLine("Process ID: " + CurrentPro.Id.ToString());

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment}.json", optional: true, reloadOnChange: true);
                    config.AddUserSecrets<Startup>();
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
            .UseStartup<Startup>()
            .UseUrls("http://localhost:5000", "https://localhost:5001");

            return host;
        }
           
    }
}
