using Microsoft.AspNetCore.Mvc;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.UI.Services.CategoryService;
using WEB_253551_KORZUN.UI.Services.ProductService;

namespace WEB_253551_KORZUN.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService,
                                 ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var productResponse = await _productService.GetProductListAsync(category, pageNo);

            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var categoriesResponse = await _categoryService.GetCategoryListAsync();

            var currentCategoryObj = categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == category);

            ViewBag.Title = "Каталог";
            ViewData["currentCategory"] = currentCategoryObj?.Name ?? "Все";
            ViewData["currentCategoryName"] = currentCategoryObj?.Name ?? "Все";
            ViewData["currentCategoryNormalizedName"] = category;
            ViewData["categories"] = categoriesResponse.Data ?? new List<Category>();

            return View(productResponse.Data);
        }
    }
}
