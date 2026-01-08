using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.UI.Services.ProductService;
using WEB_253551_KORZUN.UI.Services.CategoryService;

namespace WEB_253551_KORZUN.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public CarPart CarPart { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return Page();
            }

            var response = await _productService.CreateProductAsync(CarPart, Image);
            if (response.Successfull)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError("", response.ErrorMessage ?? "Ошибка при создании");
            await LoadCategories();
            return Page();
        }

        private async Task LoadCategories()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
        }
    }
}