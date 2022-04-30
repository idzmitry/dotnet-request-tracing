using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace Logging.Common
{
    public class CorrelationIdEnricher
    {
        private readonly ILogger _logger;
        private readonly ApiSettings _settings;
        private readonly RequestDelegate _next;

        public CorrelationIdEnricher(ILogger<CorrelationIdEnricher> logger, ApiSettings settings, RequestDelegate next)
        {
            _logger = logger;
            _settings = settings;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var xCorrelationId = context.GetXCorrelationId();
            context.SetXCorrelationId(xCorrelationId);
            using var _ = _logger.BeginScope($"{{{Constants.XCorrelationId}}} {{appName}}", xCorrelationId, _settings.AppName);
            _logger.LogInformation("Processing has started");
            await _next(context);
            _logger.LogInformation("Processing has finished");
        }
    }
}
