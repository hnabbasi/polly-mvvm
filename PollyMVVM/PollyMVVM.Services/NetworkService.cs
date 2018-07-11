using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class NetworkService : INetworkService
    {
        public async Task<T> Retry<T>(Task<T> task)
        {
            return await RetryInner(task);
        }

        public async Task<T> Retry<T>(Task<T> task, int retryCount)
        {
            return await RetryInner(task, retryCount);
        }

        public async Task<T> Retry<T>(Task<T> action,
                                                 int retryCount,
                                                 Func<Exception, int, Task> onRetry,
                                                 CancellationToken cancelToken)
        {
            return await RetryInner(action, retryCount, onRetry, cancelToken);
        }

        public async Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider);
        }

        public async Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount);
        }

        public async Task<T> WaitAndRetry<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount,
                                             Func<Exception, TimeSpan, Task> onRetryAsync, CancellationToken cancelToken)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount, onRetryAsync, cancelToken);
        }

        #region Inner Methods

        internal async Task<T> RetryInner<T>(Task<T> task,
                                                 int retryCount = 1,
                                                 Func<Exception, int, Task> onRetry = null,
                                                 CancellationToken cancelToken = default(CancellationToken))
        {
            var func = new Func<CancellationToken, Task<T>>((t) => task);
            var onRetryInner = new Func<Exception, int, Task>((e, i) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Retry #{i} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy.Handle<Exception>().RetryAsync(retryCount, onRetry ?? onRetryInner).ExecuteAsync<T>(func, cancelToken);
        }

        internal async Task<T> WaitAndRetryInner<T>(Task<T> task, Func<int, TimeSpan> sleepDurationProvider,
                                                    int retryCount = 1,
                                                    Func<Exception, TimeSpan, Task> onRetryAsync = null,
                                                    CancellationToken cancelToken = default(CancellationToken))
        {
            var func = new Func<CancellationToken, Task<T>>((t) => task);
            var f = new Func<Task<T>>(() => task);
            var onRetryInner = new Func<Exception, TimeSpan, Task>((e, t) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Retrying in {t.ToString("g")} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, sleepDurationProvider, onRetryAsync ?? onRetryInner).ExecuteAsync<T>(f);
        }

        internal async Task<T> WaitAndRetryInner<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider,
                                                    int retryCount = 1,
                                                    Func<Exception, TimeSpan, Task> onRetryAsync = null,
                                                    CancellationToken cancelToken = default(CancellationToken))
        {
            var onRetryInner = new Func<Exception, TimeSpan, Task>((e, t) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Retrying in {t.ToString("g")} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, sleepDurationProvider, onRetryAsync ?? onRetryInner).ExecuteAsync<T>(func, cancelToken);
        }

        #endregion
    }
}
