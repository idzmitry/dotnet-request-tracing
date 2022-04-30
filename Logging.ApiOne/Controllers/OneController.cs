using Logging.Common.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logging.ApiOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OneController : ControllerBase
    {
        private readonly ILogger<OneController> _logger;
        private readonly IHttpClientFactory _factory;

        public OneController(ILogger<OneController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost]
        [Route("call-fail")]
        public async Task<IActionResult> CallFail([FromBody] FailureModel failureModel)
        {
            var client = _factory.CreateClient(nameof(OneController));
            await SendRequest(failureModel, "http://api-two/two/fail");

            return Ok();
        }

        [HttpPost]
        [Route("call-process")]
        public async Task<IActionResult> CallProcess([FromBody] ProcessModel processModel)
        {
            var client = _factory.CreateClient(nameof(OneController));
            await SendRequest(processModel, "http://api-two/two/process");

            return Ok();
        }

        private async Task SendRequest(object body, string url)
        {
            using var client = _factory.CreateClient(nameof(OneController));
            await client.PostAsync(
                url,
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
        }
    }
}
