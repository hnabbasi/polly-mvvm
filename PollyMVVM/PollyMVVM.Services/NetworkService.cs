using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using PollyMVVM.Services.Abstractions;

namespace PollyMVVM.Services
{
    public class NetworkService : INetworkService
    {
        public async Task<T> Retry<T>(Func<Task<T>> func)
        {
            return await RetryInner(func);
        }

        public async Task<T> Retry<T>(Func<Task<T>> func, int retryCount)
        {
            return await RetryInner(func, retryCount);
        }

        public async Task<T> Retry<T>(Func<Task<T>> func, int retryCount, Func<Exception, int, Task> onRetry)
        {
            return await RetryInner(func, retryCount, onRetry);
        }

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider);
        }

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount);
        }

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onRetryAsync)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount, onRetryAsync);
        }

        #region Inner Methods

        internal async Task<T> RetryInner<T>(Func<Task<T>> func, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
        {
            var onRetryInner = new Func<Exception, int, Task>((e, i) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Retry #{i} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy.Handle<Exception>().RetryAsync(retryCount, onRetry ?? onRetryInner).ExecuteAsync<T>(func);
        }

        internal async Task<T> WaitAndRetryInner<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount = 1, Func<Exception, TimeSpan, Task> onRetryAsync = null)
        {
            var onRetryInner = new Func<Exception, TimeSpan, Task>((e, t) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Retrying in {t.ToString("g")} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, sleepDurationProvider, onRetryAsync ?? onRetryInner).ExecuteAsync<T>(func);
        }
        #endregion
    }
}
