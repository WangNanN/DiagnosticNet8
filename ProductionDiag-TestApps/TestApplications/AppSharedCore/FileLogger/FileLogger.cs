using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppSharedCore.FileLogger
{
    public sealed class FileLogger : ILogger
    {
        private readonly FileLoggerProvider _provider;
        private readonly string _category;

        public FileLogger(FileLoggerProvider provider, string categoryName)
        {
            _category = categoryName;
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[")
                   .Append(logLevel.ToString())
                   .Append("]")
                   .Append(_category)
                   .Append(": ")
                   .AppendLine(formatter(state, exception));

            _provider.WriteFile(builder.ToString());
        }
    }
}
