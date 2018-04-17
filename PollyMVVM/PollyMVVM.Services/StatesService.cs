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
            var states = await _apiService.GetAsync<StatesResponse>(host);

            return states.States;
        }
    }
}
