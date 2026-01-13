using WEB_253551_KORZUN.UI.HelperClasses;
using WEB_253551_KORZUN.UI.Models;
using WEB_253551_KORZUN.UI.Services;
using WEB_253551_KORZUN.UI.Services.Authentication;
using WEB_253551_KORZUN.UI.Services.Authorization;
using WEB_253551_KORZUN.UI.Services.CategoryService;
using WEB_253551_KORZUN.UI.Services.ProductService;

namespace WEB_253551_KORZUN.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

            builder.Services.AddHttpClient<IFileService, ApiFileService>(client =>
            {
                client.BaseAddress = new Uri($"{uriData.ApiUri}Files/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddHttpClient<IProductService, ApiProductService>(client =>
            {
                client.BaseAddress = new Uri(uriData.ApiUri);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(client =>
            {
                client.BaseAddress = new Uri(uriData.ApiUri);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddScoped<IAuthService, KeycloakAuthService>();

            //builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            //builder.Services.AddScoped<IProductService, MemoryProductService>();
        }
    }
}
