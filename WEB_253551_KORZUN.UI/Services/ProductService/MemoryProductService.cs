using Microsoft.AspNetCore.Mvc;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;
using WEB_253551_KORZUN.UI.Services.CategoryService;

namespace WEB_253551_KORZUN.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<CarPart> _carParts;
        List<Category> _categories;
        private readonly int _pageSize;
   
        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;

            _pageSize = config.GetValue<int>("ItemsPerPage", 3);

            SetupData();
        }

        public Task<ResponseData<CarPart>> CreateProductAsync(CarPart product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<CarPart>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<CarPart>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            IEnumerable<CarPart> query = _carParts;

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(p => p.Category != null &&
                                         p.Category.NormalizedName == categoryNormalizedName);
            }

            var filteredItems = query.ToList();

            if (pageNo < 1) pageNo = 1;

            int totalItems = filteredItems.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / _pageSize);

            if (pageNo > totalPages && totalPages > 0)
            {
                pageNo = totalPages;
            }

            var pagedItems = filteredItems
                .Skip((pageNo - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var listModel = new ListModel<CarPart>
            {
                Items = pagedItems,
                CurrentPage = pageNo,
                TotalPages = totalPages == 0 ? 1 : totalPages
            };

            return Task.FromResult(ResponseData<ListModel<CarPart>>.Success(listModel));
        }

        public Task UpdateProductAsync(int id, CarPart product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _carParts = new List<CarPart>
                 {
                    new CarPart {Id = 1, Name="Тормозные колодки",
                        Description="Комплект тормозных колодок",
                        Price=40, Image="Images/brake-shoe.jpeg",
                        Category=_categories.Find(c=>c.NormalizedName.Equals("brakes"))},

                    new CarPart { Id = 2, Name="Тормозные диски",
                        Description="Тормозной диск A.B.S. 17628",
                        Price=80, Image="Images/brake-discs.jpg",
                        Category=_categories.Find(c=>c.NormalizedName.Equals("brakes"))},

                    new CarPart { Id = 3, Name="Датчик уровня топлива",
                        Description="Датчик уровня топлива (Для а/м моделей: ВАЗ 2101, 2103, 2105, 2106, 2107)",
                        Price=46, Image="Images/fuel-level-sensor.jpg",
                        Category=_categories.Find(c=>c.NormalizedName.Equals("electrics"))},


                    new CarPart { Id = 4, Name="Диск сцепления",
                        Description="Диск сцепления нажимной 2109-1601085",
                        Price=13, Image="Images/clutch-disc.jpg",
                        Category=_categories.Find(c=>c.NormalizedName.Equals("transmission"))},

                    new CarPart { Id = 5, Name="Амортизатор передний масляный",
                        Description="Амортизатор передний масляный (Для а/м моделей: ВАЗ 2101-2107)",
                        Price=57, Image="Images/front-shock-absorber.jpg",
                        Category=_categories.Find(c=>c.NormalizedName.Equals("suspension"))},

            };

        }
    }
}
