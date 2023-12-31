using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppSharedCore.FileLogger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppDiagnostics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                                  .AddConsole()
                                  .AddDebug()
                                  .AddFile();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
