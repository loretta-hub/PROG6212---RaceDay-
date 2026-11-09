using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

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

        // Upload an image through the API
        public async Task<HttpResponseMessage> UploadFileAsync(
            string endpoint,
            IFormFile file,
            string? apiCookie = null)
        {
            using var content = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();

            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            using var request =
                new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = content
                };

            AddApiCookie(request, apiCookie);

            return await _httpClient.SendAsync(request);
        }

        private static void AddApiCookie(
            HttpRequestMessage request, string? apiCookie)
        {
            if (!string.IsNullOrWhiteSpace(apiCookie))
            {
                request.Headers.TryAddWithoutValidation(
                    "Cookie", apiCookie);
            }
        }
    }
}
