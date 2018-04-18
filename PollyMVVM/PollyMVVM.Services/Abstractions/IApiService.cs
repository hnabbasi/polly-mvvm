using System;
using System.Threading;
using System.Threading.Tasks;
using PollyMVVM.Common;

namespace PollyMVVM.Services.Abstractions
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(Uri uri) where T : class;

        #region With Retries

        Task<T> GetAndRetry<T>(Uri uri, int retryCount, Func<Exception, int, Task> onRetry = null, 
                               CancellationToken cancelToken = default(CancellationToken)) where T : class;

        Task<T> GetWaitAndTry<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onWaitAndRetry = null,
                                 CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        #endregion

    }
}
