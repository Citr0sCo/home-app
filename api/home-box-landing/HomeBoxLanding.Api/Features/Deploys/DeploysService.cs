using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Builds;
using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;
using HomeBoxLanding.Api.Features.WebSockets.Types;

namespace HomeBoxLanding.Api.Features.Deploys;

public class DeployService : ISubscriber
{
    private readonly IShellService _shellService;
    private readonly IDeployRepository _deployRepository;
    private readonly BuildsService _buildsService;

    public DeployService(IShellService shellService, IDeployRepository deployRepository, BuildsService buildsService)
    {
        _shellService = shellService;
        _deployRepository = deployRepository;
        _buildsService = buildsService;
    }

    public GetAllDeploysResponse GetAllDeploys()
    {
        var response = new GetAllDeploysResponse();
            
        var getAllDeploysResponse = _deployRepository.GetAllDeploys();

        if (getAllDeploysResponse.HasError)
        {
            response.AddError(getAllDeploysResponse!.Error!);
            return response;
        }

        response.Deploys = getAllDeploysResponse.Deploys.ConvertAll(x => new DeployModel
        {
            Identifier = x.Identifier,
            CommitId = x.CommitId,
            StartedAt = x.StartedAt,
            FinishedAt = x.FinishedAt
        });
        return response;
    }

    public GitlabBuildResponse Deploy(GithubBuildRequest request)
    {
        var response = new GitlabBuildResponse();

        var buildIdentifier = Guid.Empty;

        var existingBuild = _buildsService.GetBuild(request.WorkflowRun.HeadSha);

        if (existingBuild.HasError)
        {
            if (existingBuild.Error.Code == ErrorCode.BuildNotFound)
            {
                var newBuild = _buildsService.SaveBuild(new SaveBuildRequest
                {
                    StartedAt = request.WorkflowRun.CreatedAt,
                    Status = BuildStatusMapper.Map(request.WorkflowRun.Status),
                    Conclusion = BuildConclusionMapper.Map(request.WorkflowRun.Conclusion),
                    GithubBuildReference = request.WorkflowRun.HeadSha
                });

                if (newBuild.HasError)
                {
                    response.AddError(newBuild.Error);
                    return response;
                }

                buildIdentifier = newBuild.BuildIdentifier;
            }
            else
            {
                if (existingBuild.HasError)
                {
                    response.AddError(existingBuild.Error);
                    return response;
                }
            }
        }
        else
        {
            buildIdentifier = existingBuild.Build.Identifier;
        }

        var updateBuild = _buildsService.UpdateBuild(new UpdateBuildRequest
        {
            Identifier = buildIdentifier,
            FinishedAt = request.WorkflowRun.Status == "completed" ? request.WorkflowRun.UpdatedAt : null,
            Conclusion = BuildConclusionMapper.Map(request.WorkflowRun.Conclusion),
            Status = BuildStatusMapper.Map(request.WorkflowRun.Status)
        });
                
        if (updateBuild.HasError)
        {
            response.AddError(updateBuild.Error);
            return response;
        }
            
        if (request.WorkflowRun.Status != "completed" || request.WorkflowRun.Conclusion != "success")
            return response.WithMessage($"Not deploying due to status being '{request.WorkflowRun.Status}' and conclusion being '{request.WorkflowRun.Conclusion}'.");

        response.Message = $"Deploying because status is '{request.WorkflowRun.Status}' and conclusion is '{request.WorkflowRun.Conclusion}'.";
            
        var currentDeploys = _deployRepository.GetAllDeploys();

        if (currentDeploys.HasError || (currentDeploys.Deploys.Count > 0 && currentDeploys.Deploys.FirstOrDefault()?.FinishedAt == null))
        {
            response.AddError(new Error
            {
                Code = ErrorCode.AppAlreadyDeploying,
                UserMessage = "Application is already being deployed",
                TechnicalMessage = "Application currently being deployed. Please add queueing to prevent this error from being returned."
            });
            return response;
        }
            
        var saveDeployResponse = _deployRepository.SaveDeploy(request.WorkflowRun.HeadSha);

        if (saveDeployResponse.HasError)
        {
            response.AddError(saveDeployResponse.Error);
            return response;
        }

        WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.DeployStarted, new {
            Identifier = saveDeployResponse.DeployIdentifier,
            CommitId = saveDeployResponse.CommitId,
            StartedAt = saveDeployResponse.StartedAt
        });
            
        Task.Run(() =>
        {
            _shellService.RunOnHost($"echo \"{saveDeployResponse.DeployIdentifier}\" > /home/miloszdura/tools/docker/home-box-landing/deploying.txt");
            _shellService.RunOnHost($"cd /home/miloszdura/tools/docker/home-box-landing && bash deploy.sh {request.WorkflowRun.HeadSha}");
        });

        return response;
    }

    public void OnStarted()
    {
        try
        {
            var deployId = File.ReadAllText("/host/tools/docker/home-box-landing/deploying.txt");

            if (Guid.TryParse(deployId, out var parsedDeployId))
            {
                var finishedAt = DateTime.UtcNow;
                    
                _deployRepository.SetDeployAsFinished(parsedDeployId, finishedAt);

                WebSockets.WebSocketManager.Instance().SendToAllClients(WebSocketKey.DeployUpdated, new {
                    Identifier = parsedDeployId,
                    FinishedAt = finishedAt
                });
            }
        }
        catch (Exception)
        {
                
        }
    }

    public void OnStopping()
    {
            
    }

    public void OnStopped()
    {
            
    }
}