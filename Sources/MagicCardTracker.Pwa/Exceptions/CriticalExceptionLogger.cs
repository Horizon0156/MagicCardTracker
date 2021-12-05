using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MagicCardTracker.Pwa.Exceptions
{
    internal sealed class CriticalExceptionLogger : ILogger
    {
        private readonly NavigationManager _navigationManager;

        public CriticalExceptionLogger(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            // Inform the user about the unexpected (allows to reload the app)
            _navigationManager.NavigateTo("yikes");
        }

        private sealed class NoopScope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
