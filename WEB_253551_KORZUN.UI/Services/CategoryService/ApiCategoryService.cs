using System.Text.Json;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiCategoryService(
            HttpClient httpClient,
            ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var url = $"{_httpClient.BaseAddress!.AbsoluteUri}categories";
            _logger.LogInformation($"Requesting categories from: {url}");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                        .ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"JSON error: {ex.Message}");
                    return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Server error: {response.StatusCode}");
            return ResponseData<List<Category>>.Error(
                $"Категории не получены. Status: {response.StatusCode}");
        }
    }
}
