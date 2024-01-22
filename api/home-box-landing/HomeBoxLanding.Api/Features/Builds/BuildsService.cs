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
    private readonly IDockerBuildsRepository _dockerBuildsRepository;

    public BuildsService(IBuildsRepository buildsRepository, IShellService shellService, IDockerBuildsRepository dockerBuildsRepository)
    {
        _buildsRepository = buildsRepository;
        _shellService = shellService;
        _dockerBuildsRepository = dockerBuildsRepository;
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

    public GetAllDockerBuildsResponse GetAllDockerBuilds()
    {
        var builds = _dockerBuildsRepository.GetAll();

        return new GetAllDockerBuildsResponse
        {
            DockerBuild = builds.ConvertAll(x => new DockerBuild
            {
                Identifier = x.Identifier,
                FinishedAt = x.FinishedAt,
                StartedAt = x.StartedAt,
                Log = x.Log
            })
        };
    }

    public void UpdateAllDockerApps()
    {
        var logFile = _shellService.Run("echo output_$(date +%Y-%m-%d-%H-%M).log").TrimEnd(Environment.NewLine.ToCharArray());
        _shellService.RunOnHost($"touch /home/miloszdura/tools/updater/{logFile}");
        _shellService.RunOnHost($"/home/miloszdura/tools/updater/update-all-via-web.sh >> /home/miloszdura/tools/updater/{logFile} 2>&1");

        var logPath = $"/host/tools/updater/{logFile}";
        var output = "";

        var startTime = DateTime.Now;
        
        while (output.Contains("DONE!") is false)
        {
            Console.WriteLine($"Checking if file exists at {logPath}...");
            
            if (File.Exists(logPath) is false)
            {
                Console.WriteLine("File doesn't exist. Sleeping for 1s...");
                Thread.Sleep(1000);
                continue;
            }
            
            Console.WriteLine("File exists. Reading content...");
            
            output = File.ReadAllTextAsync(logPath).Result;
            
            Console.WriteLine("File read. Streaming to web socket clients...");
            
            WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.DockerAppUpdateProgress,  new
            {
                Result = output,
                Finished = false
            });
            
            Console.WriteLine("File not finished. Sleeping for 1s...");
            Thread.Sleep(1000);
        }

        _dockerBuildsRepository.SaveBuild(new SaveDockerBuildRequest
        {
            StartedAt = startTime,
            FinishedAt = DateTime.Now,
            Log = output
        });
            
        WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.DockerAppUpdateProgress, new
        {
            Response = new
            {
                Data = new
                {
                    Result = output,
                    Finished = true
                }
            }
        });
    }

    public GetBuildResponse GetBuild(string? githubBuildReference)
    {
        var response = new GetBuildResponse();
        
        if (githubBuildReference == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.BuildNotFound, 
                UserMessage = "Build not found.", 
                TechnicalMessage = $"Build for reference '{githubBuildReference}' not found."
            });
            return response;
        }

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
                    UserMessage = "Failed to update a build.",
                    TechnicalMessage = "Failed to update a build."
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