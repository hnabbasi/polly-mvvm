using System;
using System.Threading;
using System.Threading.Tasks;
using PollyMVVM.Common;

namespace PollyMVVM.Services.Abstractions
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(Uri uri) where T : class;

        Task<T> GetAndRetry<T>(Uri uri, int retryCount, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        Task<T> Post<T>(Uri uri, string json, string contentType = AppConstants.ContentType) where T : class;

        Task<T> PostAndRetry<T>(Uri uri, string json, int retryCount, string contentType = AppConstants.ContentType, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;
    }
}
