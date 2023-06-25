using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppSharedCore.FileLogger
{
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(0);
        private ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();
        private CancellationTokenSource _source = new CancellationTokenSource();
        private StreamWriter _logFile;

        public FileLoggerProvider()
        {
            string logPath = Path.GetTempPath();
            logPath = Path.Combine(logPath, "appLog.txt");
            FileStream fileStream = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete);
            _logFile = new StreamWriter(fileStream);
            Task.Run(() => FileWriteLoop(_source.Token), _source.Token);
        }

        private async Task FileWriteLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();

                await _semaphore.WaitAsync(token);
                if (_messages.TryDequeue(out string message))
                {
                    try
                    {
                        await _logFile.WriteAsync(message);
                        await _logFile.FlushAsync();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this, categoryName);
        }

        internal void WriteFile(string payload)
        {
            _messages.Enqueue(payload);

            try
            {
                _semaphore.Release();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void Dispose()
        {
            if (_source != null)
            {
                _source.Cancel();
                _source.Dispose();
                _source = null;
            }
            if (_semaphore != null)
            {
                _semaphore.Dispose();
                _semaphore = null;
            }
            if (_logFile != null)
            {
                _logFile.Dispose();
                _logFile = null;
            }
        }
    }
}
