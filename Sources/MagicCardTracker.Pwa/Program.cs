using Blazored.LocalStorage;
using MagicCardTracker.Pwa;
using MagicCardTracker.Pwa.Cache;
using MagicCardTracker.Pwa.Exceptions;
using MagicCardTracker.Pwa.Extensions;
using MagicCardTracker.Pwa.Helpers;
using MagicCardTracker.Pwa.Notifications;
using MagicCardTracker.Pwa.Profiles;
using MagicCardTracker.Pwa.Services.Dialogs;
using MagicCardTracker.Pwa.Settings;
using MagicCardTracker.Pwa.Storage;
using MagicCardTracker.ScryfallClient;
using MagicCardTracker.Storage;
using MagicCardTracker.Storage.Abstrations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ScryfallApiProfile>());
builder.Services.AddHttpClient();

builder.Services.AddSettings<InfoSettings>();
builder.Services.AddSettings<CollectionSettings>();
builder.Services.AddSettings<PrivacyPolicySettings>();
builder.Services.AddScoped<IUserSettings, LocalStorageUserSettings>();

builder.Services.AddSingleton<ILoggerProvider, CriticalExceptionLoggerProvider>();
builder.Services.AddTransient<IBrowserTools, JSBrowserTools>();

builder.Services.AddScoped<IScryfallClientFactory, ScryfallClientFactory>();
builder.Services.AddScoped<ICardLibraryPersister, LocalStorageCardLibrary>();
builder.Services.AddScoped<ICardLibrary, SingleUserCardLibrary>();
builder.Services.AddScoped<IObjectCache, LocalStorageCache>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDialogService, DialogService>();

var host = builder.Build();
var settings = host.Services.GetService<IUserSettings>();

if (settings != null)
{
    await settings.LoadSettingsAsync(CancellationToken.None);
}
await host.RunAsync();