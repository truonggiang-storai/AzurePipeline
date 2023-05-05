using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApi.Models;
using WebApi.Options;
using WebApi.Services.WebApi;

namespace WebApi.Services.UserApi
{
    public class UserApiService : WebApiClient, IUserApiService
    {
        private readonly UserUrlApi _userUrlApi;

        public UserApiService(IOptions<UserUrlApi> options,
            ILogger<UserApiService> logger,
            HttpClient httpClient) : base(logger, httpClient)
        {
            _userUrlApi = options.Value;
        }

        public async Task<IEnumerable<User>> ListUsersByIdsAsync(List<long> ids)
        {
            string providerName = _userUrlApi.ProviderName;
            string requestUri = $"{_userUrlApi.ApiUrl}users/ids";
            FluentUriBuilder request = CreateRequest(requestUri);

            RemoveDefaultRequestHeaders();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.mywebgrocer.session"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", _userUrlApi.Authorization);

            IEnumerable<User> response = await PostAsync<List<long>, IEnumerable<User>>(
                $"Get Users by IDs {requestUri}",
                request.Uri,
                ids,
                CancellationToken.None,
                DataInterchangeFormat.Json,
                providerName);
            return response;
        }

        private void RemoveDefaultRequestHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
