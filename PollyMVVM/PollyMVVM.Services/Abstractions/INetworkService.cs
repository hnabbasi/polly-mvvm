using System;
using System.Threading;
using System.Threading.Tasks;

namespace PollyMVVM.Services.Abstractions
{
    public interface INetworkService
    {
        Task<T> Retry<T>(Task<T> task);
        Task<T> Retry<T>(Task<T> task, int retryCount);
        Task<T> Retry<T>(Task<T> task, int retryCount, Func<Exception, int, Task> onRetry, CancellationToken cancelToken);
        Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider);
        Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount);
        Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider,
                                                    int retryCount,
                                                    Func<Exception, TimeSpan, Task> onRetryAsync,
                                                    CancellationToken cancelToken);
    }
}
