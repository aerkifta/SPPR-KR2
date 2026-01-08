using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.UI.Services.ProductService;
using WEB_253551_KORZUN.UI.Services.CategoryService;

namespace WEB_253551_KORZUN.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public CarPart CarPart { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Query.TryGetValue("id", out var idValue) ||
                !int.TryParse(idValue, out int id))
            {
                return NotFound("ID не указан");
            }

            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound("Запчасть не найдена");
            }

            CarPart = response.Data;

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

            await _productService.UpdateProductAsync(CarPart.Id, CarPart, Image);
            return RedirectToPage("./Index");
        }

        private async Task LoadCategories()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            Categories = new SelectList(categoriesResponse.Data ?? new List<Category>(),
                "Id", "Name", CarPart?.CategoryId);
        }
    }
}