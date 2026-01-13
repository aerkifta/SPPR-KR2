using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using WEB_253551_KORZUN.UI.HelperClasses;

namespace WEB_253551_KORZUN.UI.Services.Authentication
{
    public class KeycloakTokenAccessor : ITokenAccessor
    {
        private readonly KeycloakData _keycloakData;
        private readonly HttpContext? _httpContext;
        private readonly HttpClient _httpClient;

        public KeycloakTokenAccessor(
            IOptions<KeycloakData> options,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _keycloakData = options.Value;
            _httpContext = httpContextAccessor.HttpContext;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // Если пользователь вошел в систему, получить его токен
            if (_httpContext?.User.Identity?.IsAuthenticated == true)
            {
                return await _httpContext.GetTokenAsync("access_token");
            }

            // Если пользователь не входит в систему, получить токен клиента

            // Keycloak token endpoint
            var requestUrl =
                $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";

            // Http request content
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret)
            });

            // send request
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            // extract access token from response
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(jsonString)?["access_token"]?.GetValue<string>()
                ?? throw new InvalidOperationException("Access token not found in response");
        }

        public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            string token = await GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
