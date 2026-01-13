using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WEB_253551_KORZUN.Blazor;
using WEB_253551_KORZUN.Blazor.Components;
using WEB_253551_KORZUN.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IDataService, DataService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUri"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
