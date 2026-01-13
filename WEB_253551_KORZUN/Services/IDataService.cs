using WEB_253551_KORZUN.Domain.Entities;

namespace WEB_253551_KORZUN.Blazor.Services
{
    public interface IDataService
    {
        // Событие, генерируемое при загрузке данных
        event Action? DataLoaded;

        // Список категорий
        List<Category> Categories { get; set; }

        // Список автозапчастей
        List<CarPart> CarParts { get; set; }

        // Признак успешного ответа от API
        bool Success { get; set; }

        // Сообщение об ошибке
        string ErrorMessage { get; set; }

        // Пагинация
        int TotalPages { get; set; }
        int CurrentPage { get; set; }

        // Выбранная категория
        Category? SelectedCategory { get; set; }

        /// <summary>
        /// Получение списка автозапчастей
        /// </summary>
        Task GetCarPartListAsync();

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        Task GetCategoryListAsync();
    }
}
