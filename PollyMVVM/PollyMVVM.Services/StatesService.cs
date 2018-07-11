using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PollyMVVM.Common;
using PollyMVVM.Models;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class StatesService : IStatesService
    {
        readonly IApiService _apiService;
        int DEFAULT_COUNT = 3;

        public StatesService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<State>> GetStates()
        {
            var host = new Uri(AppConstants.GetStatesApi);
            var response = await _apiService.GetAsync<StatesResponse>(host);
            return response.States;
        }

        #region Retry
        public async Task<List<State>> GetStatesWithRetry()
        {
            var host = new Uri(AppConstants.GetStatesApi);
            var response = await _apiService.GetAndRetry<StatesResponse>(host, DEFAULT_COUNT, OnRetry);
            return response.States;
        }

        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Retry - Attempt #{retryCount} to get states.");
            });
        }
        #endregion

        #region WaitAndRetry
        public async Task<List<State>> GetStatesWithWaitAndRetry()
        {
            var host = new Uri(AppConstants.GetStatesApi);

            var response = await _apiService.GetWaitAndRetry<StatesResponse>(host, GetSleepDuration, DEFAULT_COUNT, OnWaitAndRetry);

            return response.States;
        }

        TimeSpan GetSleepDuration(int retryCount)
        {
            return TimeSpan.FromSeconds(retryCount * 5);
        }

        Task OnWaitAndRetry(Exception e, TimeSpan timeSpan)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"WaitAndRetry - Trying to get states in {timeSpan.ToString("g")}");
            });
        }
        #endregion

    }
}
