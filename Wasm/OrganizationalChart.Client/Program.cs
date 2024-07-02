using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using OrganizationalChart.Shared;

    var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped<SampleService>();
builder.Services.AddScoped<SfDialogService>();
await builder.Build().RunAsync();
