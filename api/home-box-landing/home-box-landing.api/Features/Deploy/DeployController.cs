using System.Net;
using home_box_landing.api.Attributes;
using home_box_landing.api.Core.Shell;
using home_box_landing.api.Features.Deploy.Types;
using Microsoft.AspNetCore.Mvc;

namespace home_box_landing.api.Features.Deploy
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class DeployController : Controller
    {
        private readonly DeployService _deployService;

        public DeployController()
        {
            _deployService = new DeployService(new ShellService(), new DeployRepository());
        }
        
        [HttpPost("")]
        [GithubAuth]
        public ActionResult Get([FromBody]GithubBuildRequest request)
        {
            var deployResponse = _deployService.Deploy(request);

            if (deployResponse.HasError)
                return StatusCode((int)HttpStatusCode.BadRequest, deployResponse.Error.UserMessage);
            
            return Ok("A-OK");
        }
    }
}