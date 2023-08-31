using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Builds.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Builds;

[ApiController]
[Route("api/[controller]")]
public class BuildsController : ControllerBase
{
    private readonly BuildsService _service;

    public BuildsController()
    {
        _service = new BuildsService(new BuildsRepository(), ShellService.Instance());
    }

    [HttpGet]
    public BuildsResponse GetAll()
    {
        return _service.GetAllBuilds();
    }

    [HttpPost("docker-apps")]
    public async Task<UpdateAllDockerAppsResponse> UpdateAllDockerApps()
    {
        return new UpdateAllDockerAppsResponse
        {
            Result = await _service.UpdateAllDockerApps()
        };
    }

    [HttpPost("run")]
    public async Task<RunOnHostResponse> UpdateAllDockerApps([FromBody] RunOnHostRequest request)
    {
        return new RunOnHostResponse
        {
            Result = await _service.RunCommandOnHost(request.Command)
        };
    }
}

public class UpdateAllDockerAppsResponse
{
    public string Result { get; set; }
}

public class RunOnHostRequest
{
    public string Command { get; set; }
}

public class RunOnHostResponse
{
    public string Result { get; set; }
}