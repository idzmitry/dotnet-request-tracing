using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Common
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _accessor;

        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            string requestContent = string.Empty;
            if (request.Content != null)
            {
                requestContent = await request.Content.ReadAsStringAsync();
            }

            _logger.LogInformation("HttpClient. method: {method} uri: {uri} payload: {payload}",
                request.Method.ToString(), request.RequestUri.ToString(), requestContent);

            var xCorrelationId = _accessor.HttpContext.GetXCorrelationId();
            request.Headers.Add(Constants.XCorrelationId, xCorrelationId);

            var response = await base.SendAsync(request, token);
            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Got response code: {code}, payload: {payload}", response.StatusCode, responseContent);
            }

            return response;
        }
    }
}
