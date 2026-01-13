using System.Text;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.Blazor.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;

        public event Action? DataLoaded;

        public List<Category> Categories { get; set; } = new();
        public List<CarPart> CarParts { get; set; } = new();

        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;

        public Category? SelectedCategory { get; set; }

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _pageSize = configuration["ItemsPerPage"] ?? "3";
        }

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        public async Task GetCategoryListAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>("categories");

                if (response is not null && response.Successfull)
                {
                    Categories = response.Data ?? new();
                    Success = true;
                }
                else
                {
                    Success = false;
                    ErrorMessage = response?.ErrorMessage ?? "Ошибка загрузки категорий";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorMessage = ex.Message;
            }

            DataLoaded?.Invoke();
        }

        /// <summary>
        /// Получение списка автозапчастей
        /// </summary>
        public async Task GetCarPartListAsync()
        {
            try
            {
                var route = new StringBuilder("carparts/");

                // категория в маршруте
                if (SelectedCategory is not null)
                {
                    route.Append($"{SelectedCategory.NormalizedName}/");
                }

                var query = new List<KeyValuePair<string, string>>();

                if (CurrentPage > 1)
                {
                    query.Add(KeyValuePair.Create("pageNo", CurrentPage.ToString()));
                }

                if (_pageSize != "3")
                {
                    query.Add(KeyValuePair.Create("pageSize", _pageSize));
                }

                if (query.Any())
                {
                    route.Append(QueryString.Create(query));
                }

                var response =
                    await _httpClient.GetFromJsonAsync<ResponseData<ListModel<CarPart>>>(route.ToString());

                if (response is not null && response.Successfull)
                {
                    CarParts = response.Data?.Items ?? new();
                    TotalPages = response.Data?.TotalPages ?? 1;
                    CurrentPage = response.Data?.CurrentPage ?? 1;
                    Success = true;
                }
                else
                {
                    Success = false;
                    ErrorMessage = response?.ErrorMessage ?? "Ошибка загрузки автозапчастей";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorMessage = ex.Message;
            }

            DataLoaded?.Invoke();
        }
    }
}
