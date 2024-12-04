
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Azure;

namespace eShopSolution.AdminApp.Services
{
    public class BrandApiClient : ApiClientBase, IBrandApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public BrandApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<bool>> Create(BrandCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            var response = await client.PostAsync($"/api/brands", requestContent);
            var result = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }


        public async Task<ApiResult<bool>> Delete(int Id)
        {
            return await DeleteAsync<ApiResult<bool>>($"/api/brands/{Id}");
        }

        public async Task<ApiResult<List<BrandViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<BrandViewModel>>>("/api/brands");
        }

        public async Task<ApiResult<BrandViewModel>> GetById(int Id)
        {
            return await GetAsync<ApiResult<BrandViewModel>>($"/api/brands/{Id}");
        }

        public async Task<ApiResult<bool>> Update(BrandUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Id.ToString()), "id");


            var response = await client.PutAsync($"/api/brands", requestContent);
            var result = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }
    }
}
