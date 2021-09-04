using System.Threading.Tasks;
using AutoMapper;
using Blazored.LocalStorage;
using MagicCardTracker.Pwa.Cache;
using MagicCardTracker.Pwa.Exceptions;
using MagicCardTracker.Pwa.Extensions;
using MagicCardTracker.Pwa.Helpers;
using MagicCardTracker.Pwa.Notifications;
using MagicCardTracker.Pwa.Profiles;
using MagicCardTracker.Pwa.Settings;
using MagicCardTracker.Pwa.Storage;
using MagicCardTracker.ScryfallClient;
using MagicCardTracker.Storage;
using MagicCardTracker.Storage.Abstrations;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MagicCardTracker.Pwa
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddAutoMapper(SetupMappings);
            builder.Services.AddHttpClient();

            builder.Services.AddSettings<InfoSettings>();
            builder.Services.AddSettings<CollectionSettings>();
            builder.Services.AddSettings<PrivacyPolicySettings>();

            builder.Services.AddSingleton<ILoggerProvider, CriticalExceptionLoggerProvider>();
            builder.Services.AddTransient<IBrowserTools, JSBrowserTools>();

            builder.Services.AddScoped<IScryfallClientFactory, ScryfallClientFactory>();
            builder.Services.AddScoped<ICardLibraryPersister, LocalStorageCardLibrary>();
            builder.Services.AddScoped<ICardLibrary, SingleUserCardLibrary>();
            builder.Services.AddScoped<IObjectCache, LocalStorageCache>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            await builder.Build().RunAsync();
        }

        private static void SetupMappings(IMapperConfigurationExpression config)
        {
            config.AddProfile<ScryfallApiProfile>();
        }
    }
}
