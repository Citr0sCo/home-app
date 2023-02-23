using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Builds;
using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;

namespace HomeBoxLanding.Api.Features.Deploys
{
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
                response.AddError(getAllDeploysResponse.Error);
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

            var existingBuild = _buildsService.GetBuild(request.workflow_run.head_sha);

            if (existingBuild.HasError)
            {
                if (existingBuild.Error.Code == ErrorCode.BuildNotFound)
                {
                    var newBuild = _buildsService.SaveBuild(new SaveBuildRequest
                    {
                        StartedAt = request.workflow_run.created_at,
                        Status = BuildStatusMapper.Map(request.workflow_run.status),
                        Conclusion = BuildConclusionMapper.Map(request.workflow_run.conclusion),
                        GithubBuildReference = request.workflow_run.head_sha
                    });

                    if (newBuild.HasError)
                    {
                        response.AddError(newBuild.Error);
                        return response;
                    }
                }
                
                if (existingBuild.HasError)
                {
                    response.AddError(existingBuild.Error);
                    return response;
                }
            }

            var updateBuild = _buildsService.UpdateBuild(new UpdateBuildRequest
            {
                Identifier = existingBuild.Build.Identifier,
                FinishedAt = request.workflow_run.status == "completed" ? request.workflow_run.updated_at : null,
                Conclusion = BuildConclusionMapper.Map(request.workflow_run.conclusion),
                Status = BuildStatusMapper.Map(request.workflow_run.status)
            });
                
            if (updateBuild.HasError)
            {
                response.AddError(updateBuild.Error);
                return response;
            }
            
            if (request.workflow_run.status != "completed" || request.workflow_run.conclusion != "success")
                return response.WithMessage($"Not deploying due to status being '{request.workflow_run.status}' and conclusion being '{request.workflow_run.conclusion}'.");

            response.Message = $"Deploying because status is '{request.workflow_run.status}' and conclusion is '{request.workflow_run.conclusion}'.";
            
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
            
            var saveDeployResponse = _deployRepository.SaveDeploy(request.workflow_run.head_sha);

            if (saveDeployResponse.HasError)
            {
                response.AddError(saveDeployResponse.Error);
                return response;
            }
            
            Task.Run(() =>
            {
                _shellService.RunOnHost($"echo \"{saveDeployResponse.DeployIdentifier}\" > /home/miloszdura/tools/docker/home-box-landing/deploying.txt");
                _shellService.RunOnHost($"cd /home/miloszdura/tools/docker/home-box-landing && bash deploy.sh {request.workflow_run.head_sha}");
            });

            return response;
        }

        public void OnStarted()
        {
            try
            {
                var deployId = File.ReadAllText("/host/tools/docker/home-box-landing/deploying.txt");

                if (Guid.TryParse(deployId, out var parsedDeployId))
                    _deployRepository.SetDeployAsFinished(parsedDeployId, DateTime.UtcNow);
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
}