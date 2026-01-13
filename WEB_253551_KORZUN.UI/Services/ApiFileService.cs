using Microsoft.AspNetCore.Http;
using WEB_253551_KORZUN.UI.Services.Authentication;

namespace WEB_253551_KORZUN.UI.Services
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient,
            ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result = result.Trim('"', '\'');

                return result;
            }
            return string.Empty;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            await _httpClient.DeleteAsync($"{fileName}");
        }
    }
}