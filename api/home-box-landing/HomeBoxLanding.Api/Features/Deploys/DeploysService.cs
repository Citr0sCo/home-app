using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;

namespace HomeBoxLanding.Api.Features.Deploys
{
    public class DeployService : ISubscriber
    {
        private readonly IShellService _shellService;
        private readonly IDeployRepository _deployRepository;

        public DeployService(IShellService shellService, IDeployRepository deployRepository)
        {
            _shellService = shellService;
            _deployRepository = deployRepository;
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

            if (request.workflow_run.status != "completed" || request.workflow_run.conclusion != "success")
                return response.WithMessage($"Not deploying due to status being {request.workflow_run.status} and conclusion being {request.workflow_run.conclusion}.");

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
                _shellService.RunOnHost($"cd /home/miloszdura/tools/docker/home-box-landing && bash deploy.sh");
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