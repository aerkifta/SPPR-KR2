using Microsoft.EntityFrameworkCore;
using WEB_253551_KORZUN.Domain.Entities;

namespace WEB_253551_KORZUN.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Console.WriteLine($"Categories count before: {await context.Categories.CountAsync()}");
            Console.WriteLine($"CarParts count before: {await context.CarParts.CountAsync()}");

            // Выполнение миграций
            await context.Database.MigrateAsync();

            if (await context.CarParts.AnyAsync())
            {
                return;
            }

            var apiUrl = app.Configuration["ApiSettings:ApiUrl"] ?? "https://localhost:7002";

            var categories = new List<Category>
            {
                new Category { Name = "Тормозная система", NormalizedName = "brakes" },
                new Category { Name = "Двигатель", NormalizedName = "engine" },
                new Category { Name = "Подвеска", NormalizedName = "suspension" },
                new Category { Name = "Коробка передач", NormalizedName = "transmission" },
                new Category { Name = "Электрика и освещение", NormalizedName = "electrics" },
                new Category { Name = "Кузов и составляющие", NormalizedName = "body-components" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            var carParts = new List<CarPart>
            {
                new CarPart
                {
                    Name = "Тормозные колодки",
                    Description = "Комплект тормозных колодок",
                    Price = 40,
                    Image = $"{apiUrl}/Images/brake-shoe.jpeg",
                    MimeType = "image/jpeg",
                    CategoryId = categories[0].Id
                },
                new CarPart
                {
                    Name = "Тормозные диски",
                    Description = "Тормозной диск A.B.S. 17628",
                    Price = 80,
                    Image = $"{apiUrl}/Images/brake-discs.jpg",
                    MimeType = "image/jpeg",
                    CategoryId = categories[0].Id
                },
                new CarPart
                {
                    Name = "Датчик уровня топлива",
                    Description = "Датчик уровня топлива (Для а/м моделей: ВАЗ 2101, 2103, 2105, 2106, 2107)",
                    Price = 46,
                    Image = $"{apiUrl}/Images/fuel-level-sensor.jpg",
                    MimeType = "image/jpeg",
                    CategoryId = categories[4].Id
                },
                new CarPart
                {
                    Name = "Диск сцепления",
                    Description = "Диск сцепления нажимной 2109-1601085",
                    Price = 13,
                    Image = $"{apiUrl}/Images/clutch-disc.jpg",
                    MimeType = "image/jpeg",
                    CategoryId = categories[3].Id
                },
                new CarPart
                {
                    Name = "Амортизатор передний масляный",
                    Description = "Амортизатор передний масляный (Для а/м моделей: ВАЗ 2101-2107)",
                    Price = 57,
                    Image = $"{apiUrl}/Images/front-shock-absorber.jpg",
                    MimeType = "image/jpeg",
                    CategoryId = categories[2].Id
                }
            };

            context.CarParts.AddRange(carParts);
            await context.SaveChangesAsync();

            Console.WriteLine($"База данных заполнена: {categories.Count} категорий, {carParts.Count} товаров");
            Console.WriteLine($"API URL: {apiUrl}");
        }
    }
}

