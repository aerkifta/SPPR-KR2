using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Тормозная система", NormalizedName="brakes"},
                new Category {Id=2, Name="Двигатель", NormalizedName="engine"},
                new Category {Id=3, Name="Подвеска", NormalizedName="suspension"},
                new Category {Id=4, Name="Коробка передач", NormalizedName="transmission"},
                new Category {Id=5, Name="Электрика и освещение", NormalizedName="electrics"},
                new Category {Id=6, Name="Кузов и составляющие", NormalizedName="body-components"}
            };
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
