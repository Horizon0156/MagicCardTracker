using System.Threading.Tasks;
using AutoMapper;
using Blazored.LocalStorage;
using Horizon.MagicCardTracker.Pwa.Cache;
using Horizon.MagicCardTracker.Pwa.Profiles;
using Horizon.MagicCardTracker.Pwa.Storage;
using Horizon.MagicCardTracker.ScryfallClient;
using Horizon.MagicCardTracker.Storage;
using Horizon.MagicCardTracker.Storage.Abstrations;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Horizon.MagicCardTracker.Pwa
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddAutoMapper(SetupMappings);
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IScryfallClientFactory, ScryfallClientFactory>();
            builder.Services.AddScoped<ICardLibraryPersister, LocalStorageCardLibrary>();
            builder.Services.AddScoped<ICardLibrary, SingleUserCardLibrary>();
            builder.Services.AddScoped<IObjectCache, LocalStorageCache>();

            await builder.Build().RunAsync();
        }

        private static void SetupMappings(IMapperConfigurationExpression config)
        {
            config.AddProfile<ScryfallApiProfile>();
        }
    }
}
