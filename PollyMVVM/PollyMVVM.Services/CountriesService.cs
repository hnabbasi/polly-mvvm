using System;
using System.Threading.Tasks;
using PollyMVVM.Common;
using PollyMVVM.Models;
using PollyMVVM.Services.Abstractions;
using Prism.Events;

namespace PollyMVVM.Services
{
    public class CountriesService : ICountriesService
    {
        readonly IApiService _apiService;
        readonly IEventAggregator _eventAggregator;
        int DEFAULT_COUNT = 3;
        Uri HOST = new Uri(AppConstants.ApiCountriesUrl);

        public CountriesService(IApiService apiService, IEventAggregator eventAggregator)
        {
            _apiService = apiService;
            _eventAggregator = eventAggregator;
        }

        public async Task<Country[]> GetCountries(){
            var response = await _apiService.GetAsync<CountriesResponse>(new Uri(AppConstants.ApiCountriesUrl));
            return response?.Countries;
        }

        #region Retry
        public async Task<Country[]> GetCountriesWithRetry()
        {
            var response = await _apiService.GetAndRetry<CountriesResponse>(HOST, DEFAULT_COUNT, OnRetry);
            return response?.Countries;
        }

        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Retry - Attempt #{retryCount} to get countries.");
            });
        }
        #endregion

        #region WaitAndRetry
        public async Task<Country[]> GetCountriesWithWaitAndRetry()
        {
            var response = await _apiService.GetAndRetry<CountriesResponse>(HOST, GetSleepDuration, DEFAULT_COUNT, OnWaitAndRetry);

            return response?.Countries;
        }

        TimeSpan GetSleepDuration(int retryCount)
        {
            // Let anyone listening know that we are about to retry.
            _eventAggregator.GetEvent<WaitRetryEvent>().Publish(retryCount);

            return TimeSpan.FromSeconds(retryCount * 3);
        }

        Task OnWaitAndRetry(Exception e, TimeSpan timeSpan)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"WaitAndRetry - Trying to get countries in {timeSpan.ToString("g")}");
            });
        }
        #endregion

    }
}
