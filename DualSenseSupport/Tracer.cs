using System;
using  System.Reactive.Disposables;
using Device.Net;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace DualSenseSupport
{
    public class Tracer : ITracer
    
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

        public void Trace(bool isWrite, byte[] data)
        {
          
        
        }
    }
}