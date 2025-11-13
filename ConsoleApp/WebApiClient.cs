using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace ConsoleApp
{
    internal class WebApiClient : IDisposable
    {

        private HttpClient _httpClient;
        public JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings();

        public WebApiClient(string baseAddress)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            var result = await _httpClient.GetAsync(requestUri);
            result.EnsureSuccessStatusCode();
            return ReadContent<T>(result.Content);
        }

        private T ReadContent<T>(HttpContent content)
        {
            var jsonString = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonString, JsonSerializerSettings)!;
        }

        public Task<T> GetAsync<T>(string requestUri, int id)
        {
            return GetAsync<T>($"{requestUri}/{id}");
        }

        public async Task<T> PostAsync<T>(string requestUri, T entity)
        {
            using StringContent content = WriteContent(entity);
            var result = await _httpClient.PostAsJsonAsync(requestUri, content);
            result.EnsureSuccessStatusCode();
            return ReadContent<T>(result.Content);
        }

        public async Task<T2> PostAsync<T1, T2>(string requestUri, T1 entity)
        {
            using StringContent content = WriteContent(entity);
            var result = await _httpClient.PostAsync(requestUri, content);
            try
            {
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var errorContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
                throw;
            }
            return ReadContent<T2>(result.Content);
        }

        public async Task PutAsync<T>(string requestUri, int id, T entity)
        {
            using StringContent content = WriteContent(entity);
            var result = await _httpClient.PutAsync($"{requestUri}/{id}", content);
            result.EnsureSuccessStatusCode();
        }

        private StringContent WriteContent<T>(T entity)
        {
            var jsonString = JsonConvert.SerializeObject(entity, JsonSerializerSettings);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return content;
        }

        public async Task DeleteAsync(string requestUri, int id)
        {
            var result = await _httpClient.DeleteAsync($"{requestUri}/{id}");
            result.EnsureSuccessStatusCode();
        }
    }
}
