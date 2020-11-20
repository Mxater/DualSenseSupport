using System;
using System.Diagnostics;
using Hid.Net.Windows;
using  System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using ILogger = Device.Net.ILogger;

namespace DSProject
{
    public class Logger : ILogger
    {
        public readonly string Name;
        

        public LogLevel LogLevel { get; set; }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => LogLevel <= logLevel;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => Disposable.Empty;

        public void Log(string message, string region, Exception ex, Device.Net.LogLevel logLevel)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
        }
    }
}