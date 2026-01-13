using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB_253551_KORZUN.UI.Extensions;
using WEB_253551_KORZUN.UI.HelperClasses;
using WEB_253551_KORZUN.UI.Models;
using WEB_253551_KORZUN.UI.Services;
using WEB_253551_KORZUN.UI.Services.CategoryService;
using WEB_253551_KORZUN.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer()
    .AddOpenIdConnect(options =>
    {
        options.Authority =
$"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
        options.ClientId = keycloakData.ClientId;
        options.ClientSecret = keycloakData.ClientSecret;
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Add("openid"); // Customize scopes as needed 
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.SaveTokens = true;
        options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
        options.MetadataAddress =
$"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";

        options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
        options.ClaimActions.MapJsonKey("avatar", "avatar");
        options.ClaimActions.MapJsonKey("email", "email");

        options.GetClaimsFromUserInfoEndpoint = true;
    });

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.RegisterCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
