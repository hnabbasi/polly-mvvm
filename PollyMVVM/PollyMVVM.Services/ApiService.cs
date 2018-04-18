using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PollyMVVM.Common;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class ApiService : IApiService
    {
        readonly IClient _client;
        readonly INetworkService _networkService;

        public ApiService(IClient client, INetworkService networkService)
        {
            _client = client;
            _networkService = networkService;
        }

        public async Task<T> GetAsync<T>(Uri uri) where T : class
        {
            return await ProcessGetRequest<T>(uri);
        }

        public async Task<T> GetAndRetry<T>(Uri uri, int retryCount, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class
        {
            return await _networkService.Retry<T>(ProcessGetRequest<T>(uri), retryCount, onRetry, cancelToken);
        }

        public Task<T> GetWaitAndTry<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onWaitAndRetry = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            return _networkService.WaitAndRetry<T>(ProcessGetRequest<T>(uri), sleepDurationProvider, retryCount, onWaitAndRetry, cancellationToken);
        }

        #region POST

        public Task<T> Post<T>(Uri uri, string json, string contentType = AppConstants.ContentType) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAndRetry<T>(Uri uri, string json, int retryCount, string contentType = AppConstants.ContentType, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Inner Methods

        async Task<T> ProcessGetRequest<T>(Uri uri)
        {
            var response = await _client.Get(uri);

            var rawResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(rawResponse);
        }

        #region POST

        async Task<T> ProcessPostRequest<T>(Uri uri, HttpContent content)
        {
            var response = await _client.Post(uri, content);

            var rawResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(rawResponse);
        }

        #endregion

        #endregion
    }
}
