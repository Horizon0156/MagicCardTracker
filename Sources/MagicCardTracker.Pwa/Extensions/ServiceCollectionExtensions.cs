
using System;
using MagicCardTracker.Pwa.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagicCardTracker.Pwa.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings<T>(
            this IServiceCollection serviceCollection) where T : SettingsBase
        {
            serviceCollection.AddSingleton<T>(sp => 
            {
                var settings = Activator.CreateInstance<T>();
                var configuration = sp.GetService<IConfiguration>();
                configuration.Bind(settings.Key, settings);

                return settings;
            });
            
            return serviceCollection;
        }
    }
}