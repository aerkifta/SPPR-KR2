using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.API.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        Task<ResponseData<ListModel<CarPart>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        Task<ResponseData<CarPart>> GetProductByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        Task UpdateProductAsync(int id, CarPart product);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        Task DeleteProductAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        Task<ResponseData<CarPart>> CreateProductAsync(CarPart product);

        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
