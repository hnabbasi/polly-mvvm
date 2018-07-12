using System;
using System.Net.Http;
using System.Threading.Tasks;
using PollyMVVM.Common;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class Client : IClient
    {
        readonly HttpClient _client;

        public Client()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(AppConstants.BaseUrl)
            };
        }

        public async Task<HttpResponseMessage> Get(Uri uri) => await _client.GetAsync(uri);
        public async Task<HttpResponseMessage> Post(Uri uri, HttpContent content) => await _client.PostAsync(uri, content);
    }
}
