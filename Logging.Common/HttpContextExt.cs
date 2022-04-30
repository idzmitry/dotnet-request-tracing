using Microsoft.AspNetCore.Http;

using System;

namespace Logging.Common
{
    public static class HttpContextExt
    {
        public static string GetXCorrelationId(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(Constants.XCorrelationId, out var xCorrelationId))
            {
                return xCorrelationId;
            }
            else
            {
                xCorrelationId = Guid.NewGuid().ToString();
            }

            return xCorrelationId;
        }

        public static void SetXCorrelationId(this HttpContext context, string xCorrelationId)
        {
            context.Response.Headers[Constants.XCorrelationId] = xCorrelationId;
        }
    }
}
