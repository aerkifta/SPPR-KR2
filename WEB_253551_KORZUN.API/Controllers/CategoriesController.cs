using Microsoft.AspNetCore.Mvc;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;
using WEB_253551_KORZUN.API.Services;

namespace WEB_253551_KORZUN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoryListAsync();

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }
    }
}
