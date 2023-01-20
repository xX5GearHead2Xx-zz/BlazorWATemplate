global using BlazorWATemplate.Shared;
global using BlazorWATemplate.Shared.DBModels;
global using System.Net.Http.Json;
global using BlazorWATemplate.Shared.DTOs.Shared;
global using BlazorWATemplate.Client.Services.AccountService;
global using Blazored.Toast.Configuration;
global using Blazored.Toast;
global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.Modal;
global using BlazorWATemplate.Shared.DTOs.Account;
using BlazorWATemplate.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
