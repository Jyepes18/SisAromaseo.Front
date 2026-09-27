using Microsoft.AspNetCore.DataProtection;
using NumbeSalud.Front.Components;
using NumbeSalud.Front.DTOs.Login;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddScoped<TokenRequest>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(
        new DirectoryInfo("/app/keys")
    )
    .SetApplicationName("NumbeSalud");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? throw new InvalidOperationException(
                     "No se configuró ApiSettings:BaseUrl."
                 );

builder.Services.AddScoped<HttpClient>(_ =>
{
    return new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true
);

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();