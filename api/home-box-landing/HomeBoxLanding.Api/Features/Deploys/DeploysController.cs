using System.Net;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Builds;
using HomeBoxLanding.Api.Features.Deploys.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Deploys;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class DeploysController : Controller
{
    private readonly DeployService _deployService;

    public DeploysController()
    {
        _deployService = new DeployService(ShellService.Instance(), new DeployRepository(), new BuildsService(new BuildsRepository(), ShellService.Instance(), new DockerBuildsRepository()));
    }
        
    [HttpGet("")]
    //[Administator]
    //[Authentication]
    public ActionResult Get()
    {
        return Ok(_deployService.GetAllDeploys());
    }
        
    [HttpPost("")]
    //[GithubAuth]
    public ActionResult Deploy([FromBody]GithubBuildRequest request)
    {
        var deployResponse = _deployService.Deploy(request);

        if (deployResponse.HasError)
            return StatusCode((int)HttpStatusCode.BadRequest, deployResponse!.Error!.UserMessage);
            
        return Ok(deployResponse.Message ?? "A-OK");
    }
}