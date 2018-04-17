using System;
using System.Threading;
using System.Threading.Tasks;

namespace PollyMVVM.Services.Abstractions
{
    public interface IApiService
    {
        /// <summary>
        /// Processes <c>Get</c> request and retries before throwing exception.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="uri">URI</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Get<T>(Uri uri) where T : class;

        /// <summary>
        /// Processes <c>Get</c> request and retries before throwing exception.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="apiPath">API path.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Get<T>(string apiPath) where T : class;

        /// <summary>
        /// Processes <c>Get</c> request and retries before throwing exception.
        /// </summary>
        /// <returns>byte[]</returns>
        /// <param name="apiPath">API path.</param>
        Task<byte[]> Get(string apiPath);

        /// <summary>
        /// Processes <c>GET</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>byte[]</returns>
        /// <param name="apiPath">API path</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        Task<byte[]> GetAndRetry(string apiPath, int retryCount = AppConstants.DefaultGetRetryCount, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Processes <c>GET</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="uri">URI</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> GetAndRetry<T>(Uri uri, int retryCount = AppConstants.DefaultGetRetryCount, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Processes <c>GET</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="apiPath">API path</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> GetAndRetry<T>(string apiPath, int retryCount = AppConstants.DefaultGetRetryCount, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// API POST. Uses Host in Settings. Serializes into JSON.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="uri">Relative path to API. Uses host stored in settings.</param>
        /// <param name="content">Content to post to API. Will be sent as JSON.</param>
        /// <param name="contentType">Content type. Default is 'application/json'</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Post<T>(Uri uri, string json, string contentType = AppConstants.ContentTypeJson) where T : class;

        /// <summary>
        /// API POST. Uses Host in Settings. Serializes into JSON.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="apiPath">API path.</param>
        /// <param name="content">Content to post to API. Will be sent as JSON.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Post<T>(string apiPath, object content) where T : class;

        /// <summary>
        /// CMS POST. Uses Host in Settings. Serializes into JSON.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="apiPath">API path.</param>
        /// <param name="content">Content to post to API. Will be sent as JSON.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> CmsPost<T>(string apiPath, object content) where T : BaseCmsResponse;

        /// <summary>
        /// Processes <c>POST</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult.</returns>
        /// <param name="uri">URI.</param>
        /// <param name="json">JSON</param>
        /// <param name="contentType">Content type. Defaults to 'application/json'</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult.</typeparam>
        Task<T> PostAndRetry<T>(Uri uri, string json, string contentType = AppConstants.ContentTypeJson, int retryCount = 1, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Processes <c>POST</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult.</returns>
        /// <param name="uri">URI.</param>
        /// <param name="content">Content. Will be sent as JSON</param>
        /// <param name="contentType">Content type. Defaults to 'application/json'</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult.</typeparam>
        Task<T> PostAndRetry<T>(Uri uri, object content, string contentType = AppConstants.ContentTypeJson, int retryCount = 1, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Processes <c>POST</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult.</returns>
        /// <param name="apiPath">API path</param>
        /// <param name="json">JSON</param>
        /// <param name="contentType">Content type. Defaults to 'application/json'</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult.</typeparam>
        Task<T> PostAndRetry<T>(string apiPath, string json, string contentType = AppConstants.ContentTypeJson, int retryCount = 1, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Processes <c>POST</c> request and retries (overriding default configuration) before throwing exception.
        /// </summary>
        /// <returns>TResult.</returns>
        /// <param name="apiPath">API path</param>
        /// <param name="content">Content. Will be sent as JSON</param>
        /// <param name="contentType">Content type. Defaults to 'application/json'</param>
        /// <param name="retryCount">Retry count.</param>
        /// <param name="onRetry">Func to execute on each retry.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <typeparam name="T">TResult.</typeparam>
        Task<T> PostAndRetry<T>(string apiPath, object content, string contentType = AppConstants.ContentTypeJson, int retryCount = 1, Func<Exception, int, Task> onRetry = null, CancellationToken cancelToken = default(CancellationToken)) where T : class;
    }
}
