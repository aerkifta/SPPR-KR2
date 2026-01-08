using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253551_KORZUN.Domain.Models;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.UI.Services.ProductService;

namespace WEB_253551_KORZUN.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }


        public ListModel<CarPart> CarParts { get;set; } = new();

        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo);
            if (response.Successfull && response.Data != null)
            {
                CarParts= response.Data;
            }
        }
    }
}
