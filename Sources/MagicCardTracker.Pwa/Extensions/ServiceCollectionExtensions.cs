using MagicCardTracker.Pwa.Settings;

namespace MagicCardTracker.Pwa.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettings<T>(
        this IServiceCollection serviceCollection) where T : SettingsBase
    {
        serviceCollection.AddSingleton<T>(sp => 
        {
            var settings = Activator.CreateInstance<T>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            configuration.Bind(settings.Key, settings);

            return settings;
        });
        
        return serviceCollection;
    }
}