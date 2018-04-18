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

            var response = await _apiService.GetAndRetry<StatesResponse>(host, 3, OnRetry);

            return response.States;
        }

        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Trying to get states #{retryCount}");
            });
        }
        #endregion

        #region WaitAndRetry
        //public async Task<List<State>> GetStatesWithRetry()
        //{
        //    var host = new Uri(AppConstants.GetStatesApi);

        //    var response = await _apiService.GetWaitAndTry<StatesResponse>(host, GetSleepDuration /*(i) => { return TimeSpan.FromSeconds(2); }*/, 3, OnWaitAndRetry);

        //    return response.States;
        //}

        //TimeSpan GetSleepDuration(int retryCount)
        //{
        //    return TimeSpan.FromSeconds(retryCount * 2);
        //}

        //Task OnWaitAndRetry(Exception e, TimeSpan timeSpan)
        //{
        //    return Task.Factory.StartNew(() => {
        //        System.Diagnostics.Debug.WriteLine($"Trying to get states in {timeSpan.ToString("g")}");
        //    });
        //}
        #endregion

    }
}
