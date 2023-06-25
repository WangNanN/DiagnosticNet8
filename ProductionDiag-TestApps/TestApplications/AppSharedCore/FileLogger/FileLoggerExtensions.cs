using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppSharedCore.FileLogger
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.Services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(ILoggerProvider), typeof(FileLoggerProvider), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton));
            return loggingBuilder;
        }
    }
}
