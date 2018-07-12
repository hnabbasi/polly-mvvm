using System;
using System.Threading.Tasks;
using PollyMVVM.Common;
using PollyMVVM.Models;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class CountriesService : ICountriesService
    {
        readonly IApiService _apiService;
        int DEFAULT_COUNT = 3;

        public CountriesService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Country[]> GetCountries(){
            var response = await _apiService.GetAsync<CountriesResponse>(new Uri(AppConstants.ApiCountriesUrl));
            var results = response.Countries;
            return results;
        }

        #region Retry
        public async Task<Country[]> GetCountriesWithRetry()
        {
            var host = new Uri(AppConstants.ApiCountriesUrl);
            var response = await _apiService.GetAndRetry<CountriesResponse>(host, DEFAULT_COUNT, OnRetry);
            return response.Countries;
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
            var host = new Uri(AppConstants.ApiCountriesUrl);

            var response = await _apiService.GetAndRetry<CountriesResponse>(host, GetSleepDuration, DEFAULT_COUNT, OnWaitAndRetry);

            return response.Countries;
        }

        TimeSpan GetSleepDuration(int retryCount)
        {
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
