using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyMVVM.Services.Abstractions
{
    public interface IClient
    {
        Task<HttpResponseMessage> Get(Uri uri);
        Task<HttpResponseMessage> Post(Uri uri, HttpContent content);
    }
}
