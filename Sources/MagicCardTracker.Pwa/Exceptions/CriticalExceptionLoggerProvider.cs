#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MagicCardTracker.Pwa.Exceptions
{
    internal sealed class CriticalExceptionLoggerProvider : ILoggerProvider
    {
        private readonly NavigationManager _navigationManager;

        public CriticalExceptionLoggerProvider(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private ILogger? _logger;

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
            {
                _logger = new CriticalExceptionLogger(_navigationManager);
            }
            return _logger;
        }

        public void Dispose()
        {
            _logger = null;
        }
    }
}
