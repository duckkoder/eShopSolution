using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();

            return token;
        }

        public async Task<PagedResult<UserVM>> GetUserPaging(GetUserPagingRequest request)
        {
            Console.WriteLine($"Keyword: '{request.Keyword}', BearerToken: '{request.BearerToken}'");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);

            string keywordParam = string.IsNullOrWhiteSpace(request.Keyword) ? string.Empty : $"&keyword={request.Keyword}";

            var response = await client.GetAsync($"/api/users/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}{keywordParam}&BearerToken={request.BearerToken}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                throw new Exception($"Error: {response.StatusCode}, Reason: {body}");
            }

            var bodyResponse = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserVM>>(bodyResponse);
            return users;
        }

    }
}
