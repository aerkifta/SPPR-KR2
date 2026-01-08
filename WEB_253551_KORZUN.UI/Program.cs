using Microsoft.EntityFrameworkCore;
using WEB_253551_KORZUN.UI.Extensions;
using WEB_253551_KORZUN.UI.Models;
using WEB_253551_KORZUN.UI.Services;
using WEB_253551_KORZUN.UI.Services.CategoryService;
using WEB_253551_KORZUN.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
