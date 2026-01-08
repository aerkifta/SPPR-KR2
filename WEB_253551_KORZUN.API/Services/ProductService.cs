using Microsoft.EntityFrameworkCore;
using WEB_253551_KORZUN.API.Data;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<CarPart>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.CarParts
                .Include(cp => cp.Category)
                .AsQueryable();

            var dataList = new ListModel<CarPart>();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(cp => cp.Category != null &&
                    cp.Category.NormalizedName == categoryNormalizedName);
            }

            var count = await query.CountAsync();

            if (count == 0)
            {
                return ResponseData<ListModel<CarPart>>.Success(dataList);
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNo > totalPages)
                return ResponseData<ListModel<CarPart>>.Error("No such page");

            dataList.Items = await query
                .OrderBy(cp => cp.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(cp => new CarPart
                {
                    Id = cp.Id,
                    Name = cp.Name,
                    Description = cp.Description,
                    Price = cp.Price,
                    CategoryId = cp.CategoryId,
                    Image = cp.Image,
                    MimeType = cp.MimeType
                })
                .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<CarPart>>.Success(dataList);
        }

        public async Task<ResponseData<CarPart>> GetProductByIdAsync(int id)
        {
            try
            {
                var carPart = await _context.CarParts
                    .Include(cp => cp.Category)
                    .FirstOrDefaultAsync(cp => cp.Id == id);

                if (carPart == null)
                {
                    return ResponseData<CarPart>.Error("Товар не найден");
                }

                return ResponseData<CarPart>.Success(carPart);
            }
            catch (Exception ex)
            {
                return ResponseData<CarPart>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(int id, CarPart product)
        {
            var existingProduct = await _context.CarParts.FindAsync(id);

            if (existingProduct == null)
            {
                throw new ArgumentException("Товар не найден");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Image = product.Image;
            existingProduct.MimeType = product.MimeType;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var carPart = await _context.CarParts.FindAsync(id);

            if (carPart == null)
            {
                throw new ArgumentException("Товар не найден");
            }

            _context.CarParts.Remove(carPart);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<CarPart>> CreateProductAsync(CarPart product)
        {
            try
            {
                _context.CarParts.Add(product);
                await _context.SaveChangesAsync();

                return ResponseData<CarPart>.Success(product);
            }
            catch (Exception ex)
            {
                return ResponseData<CarPart>.Error($"Ошибка при создании: {ex.Message}");
            }
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}