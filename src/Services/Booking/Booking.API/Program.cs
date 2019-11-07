namespace Booking.API
{
#pragma warning disable
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public class Program
    {

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
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
            .UseStartup<Startup>()
            .UseUrls("http://localhost:5002", "https://localhost:5003");

            return host;
        }


        public static void Main(string[] args)
        {
            System.Diagnostics.Process CurrentPro = new System.Diagnostics.Process();
            CurrentPro = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine("Process ID: " + CurrentPro.Id.ToString());
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}
