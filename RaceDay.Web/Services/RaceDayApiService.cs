
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
            string endpoint,
            TRequest data)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, data);
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            using var response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
