using Logging.Common.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Logging.ApiTwo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwoController : ControllerBase
    {
        private readonly ILogger<TwoController> _logger;

        public TwoController(ILogger<TwoController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("fail")]
        public async Task<IActionResult> Fail([FromBody]FailureModel failureModel)
        {
            await Task.Delay(1_000);
            if (failureModel == null || string.IsNullOrWhiteSpace(failureModel.Prop1) || string.IsNullOrWhiteSpace(failureModel.Prop2))
            {
                _logger.LogError("Ok. I'm going to fail the request. prop1: {prop1}, prop2: {prop2}",
                    failureModel?.Prop1, failureModel?.Prop2);

                throw new ArgumentException(nameof(failureModel));
            }

            return Ok();
        }

        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> Process([FromBody] ProcessModel processModel)
        {
            await Task.Delay(1_000);
            if (processModel == null)
            {
                throw new ArgumentException(nameof(processModel));
            }

            _logger.LogInformation("Processing modelId: {modelId}", processModel.Id);

            return Ok();
        }
    }
}
