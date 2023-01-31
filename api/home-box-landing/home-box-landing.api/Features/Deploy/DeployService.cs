using home_box_landing.api.Core.Shell;
using home_box_landing.api.Core.Types;
using home_box_landing.api.Features.Deploy.Types;

namespace home_box_landing.api.Features.Deploy
{
    public class DeployService
    {
        private readonly IShellService _shellService;
        private readonly IDeployRepository _deployRepository;

        public DeployService(IShellService shellService, IDeployRepository deployRepository)
        {
            _shellService = shellService;
            _deployRepository = deployRepository;
        }

        public GitlabBuildResponse Deploy(GithubBuildRequest request)
        {
            var response = new GitlabBuildResponse();

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

            
            var saveDeployResponse = _deployRepository.SaveDeploy(request.head_commit.id);

            if (saveDeployResponse.HasError)
            {
                response.AddError(saveDeployResponse.Error);
                return response;
            }

            Task.Run(() =>
            {
                _shellService.Run($"cd /home/miloszdura/tools/docker/home-box-landing && sudo bash deploy.sh > /dev/null 2>&1");
                
                var setDeployAsFinished = _deployRepository.SetDeployAsFinished(saveDeployResponse.DeployIdentifier, DateTime.Now);

                if (setDeployAsFinished.HasError)
                    response.AddError(setDeployAsFinished.Error);
            });


            return response;
        }
    }
}