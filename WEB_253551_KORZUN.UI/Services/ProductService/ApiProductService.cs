using System.Text;
using System.Text.Json;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.UI.Services.ProductService
{
 public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _pageSize;
        private readonly IFileService _fileService;

        public ApiProductService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger,
            IFileService fileService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _fileService = fileService;
            _pageSize = configuration["ItemsPerPage"] ?? "3";

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ResponseData<ListModel<CarPart>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1)
        {
            try
            {
                var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}carparts/");

                if (!string.IsNullOrEmpty(categoryNormalizedName))
                {
                    urlString.Append($"category/{Uri.EscapeDataString(categoryNormalizedName)}");
                }

                urlString.Append($"?pageNo={pageNo}");

                if (_pageSize != "3")
                {
                    urlString.Append($"&pageSize={_pageSize}");
                }

                var url = urlString.ToString();
                _logger.LogInformation($"Requesting URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content
                        .ReadFromJsonAsync<ResponseData<ListModel<CarPart>>>(_serializerOptions);

                    _logger.LogInformation($"Successfully received {result?.Data?.Items?.Count ?? 0} items");
                    return result ?? ResponseData<ListModel<CarPart>>.Error("No data received");
                }

                _logger.LogError($"Server error: {response.StatusCode}");
                return ResponseData<ListModel<CarPart>>.Error(
                    $"Ошибка сервера: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProductListAsync");
                return ResponseData<ListModel<CarPart>>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task<ResponseData<CarPart>> GetProductByIdAsync(int id)
        {
            try
            {
                var url = $"{_httpClient.BaseAddress!.AbsoluteUri}carparts/{id}";
                _logger.LogInformation($"Requesting product by ID: {id}");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<CarPart>>(_serializerOptions);
                }

                _logger.LogError($"Server error: {response.StatusCode}");
                return ResponseData<CarPart>.Error($"Ошибка: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProductByIdAsync");
                return ResponseData<CarPart>.Error($"Ошибка: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(int id, CarPart product, IFormFile? formFile)
        {
            try
            {
                var oldProductResponse = await GetProductByIdAsync(id);
                string? oldFileName = null;

                if (oldProductResponse.Successfull && oldProductResponse.Data != null)
                {
                    var oldImageUrl = oldProductResponse.Data.Image;
                    if (!string.IsNullOrEmpty(oldImageUrl) && !oldImageUrl.Contains("noimage.jpg"))
                    {
                        oldFileName = Path.GetFileName(oldImageUrl)?.Trim('"', '\'');
                    }
                }

                if (formFile != null)
                {
                    var imageUrl = await _fileService.SaveFileAsync(formFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        product.Image = imageUrl;
                    }
                }
                else
                {
                    product.Image = oldProductResponse.Data?.Image;
                }

                var url = $"{_httpClient.BaseAddress!.AbsoluteUri}carparts/{id}";
                var jsonContent = JsonSerializer.Serialize(product, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Update failed: {response.StatusCode}");
                    throw new Exception($"Update failed: {response.StatusCode}");
                }

                if (formFile != null && !string.IsNullOrEmpty(oldFileName))
                {
                    try
                    {
                        await _fileService.DeleteFileAsync(oldFileName);
                    }
                    catch
                    {
                        _logger.LogWarning($"Не удалось удалить старый файл: {oldFileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateProductAsync");
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await GetProductByIdAsync(id);
            if (response.Successfull && response.Data != null)
            {
                var product = response.Data;
                var imageUrl = product.Image;
                var fileName = Path.GetFileName(imageUrl);
                fileName = fileName?.Trim('"', '\'');

                if (!string.IsNullOrEmpty(fileName) && !imageUrl.Contains("noimage.jpg"))
                {
                    await _fileService.DeleteFileAsync(fileName);
                }
            }

            await _httpClient.DeleteAsync($"carparts/{id}");
        }

        public async Task<ResponseData<CarPart>> CreateProductAsync(CarPart product, IFormFile? formFile)
        {
            try
            {
                product.Image = "Images/noimage.jpg";
                product.MimeType = "image/jpeg";

                if (formFile != null)
                {
                    var imageUrl = await _fileService.SaveFileAsync(formFile);

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        product.Image = imageUrl;
                        product.MimeType = formFile.ContentType;
                    }
                }

                var url = $"{_httpClient.BaseAddress!.AbsoluteUri}carparts";

                var jsonContent = JsonSerializer.Serialize(product, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content
                        .ReadFromJsonAsync<ResponseData<CarPart>>(_serializerOptions);
                    return data ?? ResponseData<CarPart>.Error("No data received");
                }

                _logger.LogError($"Create failed: {response.StatusCode}");
                return ResponseData<CarPart>.Error($"Ошибка создания: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateProductAsync");
                return ResponseData<CarPart>.Error($"Ошибка: {ex.Message}");
            }
        }
    }
}

