using Microsoft.EntityFrameworkCore;
using WEB_253551_KORZUN.API.Data;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return ResponseData<List<Category>>.Success(categories);
            }
            catch (Exception ex)
            {
                return ResponseData<List<Category>>.Error($"Ошибка при получении категорий: {ex.Message}");
            }
        }
    }
}