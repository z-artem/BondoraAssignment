using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace RentalWeb.Infrastructure
{
    public class FileLogger : ILogger
    {
        private readonly string path;
        private object lockObject = new object();

        public FileLogger(string path)
        {
            this.path = path;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (lockObject)
                {
                    File.AppendAllText(path, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
