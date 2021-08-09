using System.Threading.Tasks;
using Blazored.LocalStorage;
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

            builder.Services.AddScoped<IScryfallClient>(sp => new ScryfallHttpClient());
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMediatR(typeof(Program));

            builder.Services.AddScoped<ICardLibraryPersister, LocalStorageCardLibrary>();
            builder.Services.AddScoped<ICardLibrary, SingleUserCardLibrary>();

            await builder.Build().RunAsync();
        }
    }
}
