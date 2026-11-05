using System.Net.Http.Json;
namespace RaceDay.Web.Services
{
    public class RaceDayApiService
    {
        private readonly HttpClient _httpClient;

        public RaceDayApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(
            string endpoint, TRequest data, string? apiCookie = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(data)
            };

            AddApiCookie(request, apiCookie);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync<TRequest>(
            string endpoint, TRequest data, string? apiCookie = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = JsonContent.Create(data)
            };

            AddApiCookie(request, apiCookie);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string endpoint, string? apiCookie = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

            AddApiCookie(request, apiCookie);

            return await _httpClient.SendAsync(request);
        }

        public async Task<T?> GetAsync<T>(
            string endpoint, string? apiCookie = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            AddApiCookie(request, apiCookie);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        private static void AddApiCookie(
            HttpRequestMessage request, string? apiCookie)
        {
            if (!string.IsNullOrWhiteSpace(apiCookie))
            {
                request.Headers.TryAddWithoutValidation("Cookie", apiCookie);
            }
        }
    }
}
