using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Serialization;
using WEB_253551_KORZUN.API.Data;
using WEB_253551_KORZUN.API.Models;
using WEB_253551_KORZUN.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
    });

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50MB
});

builder.Services.AddControllers();


var authServer = builder.Configuration
    .GetSection("AuthServer")
    .Get<AuthServerData>();

// Добавить сервис аутентификации 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        // Адрес метаданных конфигурации OpenID 
        o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

        // Authority сервера аутентификации 
        o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";

        // Audience для токена JWT 
        o.Audience = "account";

        // Запретить HTTPS для использования локальной версии Keycloak 
        // В рабочем проекте должно быть true 
        o.RequireHttpsMetadata = false;
    });


builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbInitializer.SeedData(app);

app.Run();
