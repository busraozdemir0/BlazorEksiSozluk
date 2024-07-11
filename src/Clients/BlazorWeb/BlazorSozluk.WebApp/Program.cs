using Blazored.LocalStorage;
using BlazorSozluk.WebApp;
using BlazorSozluk.WebApp.Infrastructure.Auth;
using BlazorSozluk.WebApp.Infrastructure.Services;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Client islemleri icin base address olusturuldu (UI tarafinda her defasinda localhost:portNo yazmamak icin)
builder.Services.AddHttpClient("WebApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7068");
    //client.BaseAddress = new Uri("http://localhost:8080"); // docker host'da calistirabilmek icin .yml uzantili dosyada belirttigimiz gibi 8080 portuna ayarliyoruz
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("WepApiClient");
});

builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<IVoteService, VoteService>();
builder.Services.AddTransient<IFavService, FavService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>(); // Birisi AuthenticationStateProvider'a ihtiyac duydugunda bizim olusturdugumuz AuthStateProvider'ý donecek

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore(); // Authentication mekanizmasini calistirabilmek icin gereklidir

await builder.Build().RunAsync();
