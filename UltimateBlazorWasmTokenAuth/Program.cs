using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UltimateBlazorWasmTokenAuth;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.Handlers;
using UltimateBlazorWasmTokenAuth.Helper;
using UltimateBlazorWasmTokenAuth.Repository;
using UltimateBlazorWasmTokenAuth.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<AuthenticationHandler>();

builder.Services.AddHttpClient("ServerApi")
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? ""))
                .AddHttpMessageHandler<AuthenticationHandler>();




builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());



builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IRepositoryManager, RepositoryManager>();

//builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddBlazoredLocalStorageAsSingleton();




await builder.Build().RunAsync();
