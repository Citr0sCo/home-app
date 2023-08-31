using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;

namespace HomeBoxLanding.Api.Features.Builds;

public class BuildsService
{
    private readonly IBuildsRepository _buildsRepository;
    private readonly IShellService _shellService;

    public BuildsService(IBuildsRepository buildsRepository, IShellService shellService)
    {
        _buildsRepository = buildsRepository;
        _shellService = shellService;
    }

    public BuildsResponse GetAllBuilds()
    {
        var builds = _buildsRepository.GetAll();

        return new BuildsResponse
        {
            Builds = builds.ConvertAll(x => new Build
            {
                Identifier = x.Identifier,
                FinishedAt = x.FinishedAt,
                StartedAt = x.StartedAt,
                Conclusion = x.Conclusion.ToString(),
                Status = x.Status.ToString()
            })
        };
    }

    public string UpdateAllDockerApps()
    {
        var logFile = _shellService.Run("echo /home/miloszdura/tools/updater/output_$(date +%Y-%m-%d-%H-%M).log");
        _shellService.RunOnHostSecondary($"/home/miloszdura/tools/updater/update-all.sh >> {logFile} 2>&1");

        return logFile;
    }

    public string RunCommandOnHost(string command)
    {
        return _shellService.RunOnHostSecondary(command);
    }

    public GetBuildResponse GetBuild(string githubBuildReference)
    {
        var response = new GetBuildResponse();

        var build = _buildsRepository.GetBuild(githubBuildReference);

        if (build == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.BuildNotFound, 
                UserMessage = "Build not found.", 
                TechnicalMessage = $"Build for reference '{githubBuildReference}' not found."
            });
            return response;
        }
        
        return new GetBuildResponse
        {
            Build = new Build
            {
                Identifier = build.Identifier,
                FinishedAt = build.FinishedAt,
                StartedAt = build.StartedAt,
                Conclusion = build.Conclusion.ToString(),
                Status = build.Status.ToString(),
                GithubBuildReference = build.GithubBuildReference
            }
        };
    }

    public SaveBuildResponse SaveBuild(SaveBuildRequest request)
    {
        var response = new SaveBuildResponse();
        
        var build = _buildsRepository.SaveBuild(request);

        if (build.HasError)
        {
            response.AddError(new Error
                {
                    Code = ErrorCode.FailedToCreateBuild,
                    UserMessage = "Failed to create a build.",
                    TechnicalMessage = "Failed to create a build."
                }
            );
            return response;
        }

        WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.BuildStarted, new {
            Identifier = build.BuildIdentifier,
            StartedAt = request.StartedAt,
            Status = request.Status.ToString()
        });
        
        return new SaveBuildResponse
        {
            BuildIdentifier = build.BuildIdentifier
        };
    }
    public UpdateBuildResponse UpdateBuild(UpdateBuildRequest request)
    {
        var response = new UpdateBuildResponse();
        
        var builds = _buildsRepository.UpdateBuild(request);

        if (builds.HasError)
        {
            response.AddError(new Error
                {
                    Code = ErrorCode.FailedToUpdateBuild,
                    UserMessage = "Failed to create a build.",
                    TechnicalMessage = "Failed to create a build."
                }
            );
            return response;
        }

        WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.BuildUpdated, new {
            Identifier = request.Identifier,
            Status = request.Status.ToString(),
            Conclusion = request.Conclusion.ToString(),
            FinishedAt = request.FinishedAt
        });
        
        return new UpdateBuildResponse
        {
            BuildIdentifier = builds.BuildIdentifier
        };
    }
}