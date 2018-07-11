using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        #region With Retries

        public async Task<T> GetAndRetry<T>(Uri uri, int retryCount, Func<Exception, int, Task> onRetry = null) where T : class
        {
            var func = new Func<Task<T>>(() => ProcessGetRequest<T>(uri));
            return await _networkService.Retry<T>(func, retryCount, onRetry);
        }

        public async Task<T> GetWaitAndRetry<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onWaitAndRetry = null) where T : class
        {
            var func = new Func<Task<T>>(() => ProcessGetRequest<T>(uri));
            return await _networkService.WaitAndRetry<T>(func, sleepDurationProvider, retryCount, onWaitAndRetry);
        }

        #endregion

        #region Inner Methods

        async Task<T> ProcessGetRequest<T>(Uri uri)
        {
            var response = await _client.Get(uri);

            var rawResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(rawResponse);
        }

        #endregion
    }
}
