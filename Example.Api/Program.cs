using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Example.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseSerilog(
                    (hostingContext, loggerConfig) =>
                        loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
                )
                .UseStartup<Startup>();
        }
    }
}
